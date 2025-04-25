using CoreSuit.Domain.DTOs.Paginate;
using CoreSuit.Domain.Requests;
using System.Linq.Expressions;

namespace CoreSuit.Application.Interfaces;

/// <summary>
/// Interface base genérica para serviços de aplicação.
/// Define contratos para operações CRUD padrão com suporte a paginação, DTOs e ativação/inativação lógica.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade do domínio.</typeparam>
/// <typeparam name="TDto">DTO utilizado para retorno de dados.</typeparam>
/// <typeparam name="TCreateDto">DTO utilizado para criação de novos registros.</typeparam>
/// <typeparam name="TUpdateDto">DTO utilizado para atualização de registros existentes.</typeparam>
public interface IBaseService<TEntity, TDto, TCreateDto, TUpdateDto>
{
    /// <summary>
    /// Retorna uma lista paginada e ordenada de registros da entidade.
    /// </summary>
    /// <typeparam name="TRequest">Tipo da requisição de paginação, derivado de <see cref="PaginationOrderRequest"/>.</typeparam>
    /// <param name="request">Objeto contendo dados de paginação, ordenação e filtros.</param>
    Task<PaginationDTO<TDto>> GetAllAsync<TRequest>(TRequest request) where TRequest : PaginationOrderRequest;

    /// <summary>
    /// Busca um único registro da entidade pelo identificador (ID).
    /// </summary>
    /// <param name="id">Identificador único do registro.</param>
    Task<TDto> GetByIdAsync(int id);

    /// <summary>
    /// Cria um novo registro no banco de dados com base no DTO de criação.
    /// </summary>
    /// <param name="request">DTO contendo os dados necessários para criação do registro.</param>
    Task CreateAsync(TCreateDto request);

    /// <summary>
    /// Atualiza um registro existente com base no DTO de atualização.
    /// </summary>
    /// <param name="request">DTO contendo os dados atualizados e o ID do registro.</param>
    Task UpdateAsync(TUpdateDto request);

    /// <summary>
    /// Inativa (soft delete) um registro, marcando a propriedade DeletedAt com a data atual.
    /// </summary>
    /// <param name="id">Identificador do registro a ser inativado.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Reativa um registro anteriormente inativado (soft delete).
    /// </summary>
    /// <param name="id">Identificador do registro a ser reativado.</param>
    Task ActiveAsync(int id);
}
