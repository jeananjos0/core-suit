using System;
using AutoMapper;
using CoreSuit.Application.Interfaces;
using CoreSuit.Domain.DTOs.FilterOptions;
using CoreSuit.Domain.DTOs.Paginate;
using CoreSuit.Domain.Interfaces.Repositories;
using CoreSuit.Domain.Requests;
using CoreSuit.Domain.Utils;

namespace CoreSuit.Application.Services;

/// <summary>
/// Serviço base genérico para operações CRUD e ativação/inativação lógica.
/// Implementa as interfaces de serviço e utiliza AutoMapper para conversão entre entidades e DTOs.
/// Permite herança para aplicação de validações específicas com os hooks protegidos.
/// </summary>
/// <typeparam name="TEntity">Entidade do domínio.</typeparam>
/// <typeparam name="TDto">DTO de retorno usado na listagem e detalhes.</typeparam>
/// <typeparam name="TCreateDto">DTO usado para criação de novos registros.</typeparam>
/// <typeparam name="TUpdateDto">DTO usado para atualização de registros existentes.</typeparam>
public class BaseService<TEntity, TDto, TCreateDto, TUpdateDto> : IBaseService<TEntity, TDto, TCreateDto, TUpdateDto>
    where TEntity : class
{
    protected readonly IBaseRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    /// <summary>
    /// Construtor principal do serviço base.
    /// </summary>
    public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retorna uma lista paginada e ordenada dos registros da entidade, mapeando para o DTO correspondente.
    /// </summary>
    /// <typeparam name="TRequest">Requisição de paginação e ordenação herdada de <see cref="PaginationOrderRequest"/>.</typeparam>
    /// <param name="request">Dados de paginação e ordenação.</param>
    public virtual async Task<PaginationDTO<TDto>> GetAllAsync<TRequest>(TRequest request)
        where TRequest : PaginationOrderRequest
    {
        IQueryable<TEntity> query = _repository.All(new FilterOptionsDTO<TEntity>());
        IQueryable<TDto> data = _mapper.ProjectTo<TDto>(query);
        return await _repository.ApplyPaginationAndOrderingAsync(data, request);
    }

    /// <summary>
    /// Busca um registro pelo ID, aplicando includes se o repositório implementar.
    /// </summary>
    public virtual async Task<TDto> GetByIdAsync(int id)
    {
        var getMethod = _repository.GetType().GetMethod("GetByIdWithIncludesAsync");
        var task = (Task<TEntity?>)getMethod!.Invoke(_repository, new object[] { id })!;
        await task.ConfigureAwait(false);
        var entity = ((dynamic)task).Result;

        if (entity == null)
            throw new KeyNotFoundException($"{typeof(TEntity).Name} não encontrado");

        await ValidateAfterGetAsync(entity);
        return _mapper.Map<TDto>(entity);
    }

    /// <summary>
    /// Cria um novo registro com base no DTO recebido.
    /// </summary>
    public virtual async Task CreateAsync(TCreateDto request)
    {
        await ValidateBeforeCreateAsync(request);
        TEntity entity = _mapper.Map<TEntity>(request);
        await _repository.AddAsync(entity);
        await ValidateAfterCreateAsync(entity);
    }

    /// <summary>
    /// Atualiza um registro existente baseado no DTO de atualização.
    /// </summary>
    public virtual async Task UpdateAsync(TUpdateDto request)
    {
        var idProperty = typeof(TUpdateDto).GetProperty("Id");
        if (idProperty == null)
            throw new InvalidOperationException("Update DTO precisa ter uma propriedade 'Id'.");

        int id = (int)idProperty.GetValue(request)!;
        TEntity? entity = await _repository.GetAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"{typeof(TEntity).Name} não encontrado");

        await ValidateBeforeUpdateAsync(request, entity);
        _mapper.Map(request, entity);

        typeof(TEntity).GetProperty("UpdatedAt")?.SetValue(entity, GetDateTimeBrasilia.Get());
        await _repository.UpdateAsync(entity);
        await ValidateAfterUpdateAsync(entity);
    }

    /// <summary>
    /// Marca um registro como inativo (soft delete) preenchendo a propriedade DeletedAt.
    /// </summary>
    public virtual async Task DeleteAsync(int id)
    {
        TEntity? entity = await _repository.GetAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"{typeof(TEntity).Name} não encontrado");

        await ValidateBeforeDeleteAsync(entity);

        typeof(TEntity).GetProperty("DeletedAt")?.SetValue(entity, GetDateTimeBrasilia.Get());
        await _repository.UpdateAsync(entity);
        await ValidateAfterDeleteAsync(entity);
    }

    /// <summary>
    /// Reativa um registro inativo (define DeletedAt como null).
    /// </summary>
    public virtual async Task ActiveAsync(int id)
    {
        TEntity? entity = await _repository.GetAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"{typeof(TEntity).Name} não encontrado");

        await ValidateBeforeActivateAsync(entity);

        typeof(TEntity).GetProperty("DeletedAt")?.SetValue(entity, null);
        await _repository.UpdateAsync(entity);
        await ValidateAfterActivateAsync(entity);
    }

    // ----------------------------
    // Métodos virtuais para validações customizadas
    // ----------------------------

    /// <summary>
    /// Validação customizada antes de criar um registro. Pode ser sobrescrita.
    /// </summary>
    protected virtual Task ValidateBeforeCreateAsync(TCreateDto request) => Task.CompletedTask;

    /// <summary>
    /// Validação customizada após criar um registro. Pode ser sobrescrita.
    /// </summary>
    protected virtual Task ValidateAfterCreateAsync(TEntity entity) => Task.CompletedTask;

    /// <summary>
    /// Validação customizada antes de atualizar um registro. Pode ser sobrescrita.
    /// </summary>
    protected virtual Task ValidateBeforeUpdateAsync(TUpdateDto request, TEntity entity) => Task.CompletedTask;

    /// <summary>
    /// Validação customizada após atualizar um registro. Pode ser sobrescrita.
    /// </summary>
    protected virtual Task ValidateAfterUpdateAsync(TEntity entity) => Task.CompletedTask;

    /// <summary>
    /// Validação customizada antes de inativar (soft delete) um registro. Pode ser sobrescrita.
    /// </summary>
    protected virtual Task ValidateBeforeDeleteAsync(TEntity entity) => Task.CompletedTask;

    /// <summary>
    /// Validação customizada após inativar (soft delete) um registro. Pode ser sobrescrita.
    /// </summary>
    protected virtual Task ValidateAfterDeleteAsync(TEntity entity) => Task.CompletedTask;

    /// <summary>
    /// Validação customizada antes de reativar um registro. Pode ser sobrescrita.
    /// </summary>
    protected virtual Task ValidateBeforeActivateAsync(TEntity entity) => Task.CompletedTask;

    /// <summary>
    /// Validação customizada após reativar um registro. Pode ser sobrescrita.
    /// </summary>
    protected virtual Task ValidateAfterActivateAsync(TEntity entity) => Task.CompletedTask;

    /// <summary>
    /// Validação customizada após carregar um registro. Pode ser usada para aplicar verificações adicionais.
    /// </summary>
    protected virtual Task ValidateAfterGetAsync(TEntity entity) => Task.CompletedTask;
}
