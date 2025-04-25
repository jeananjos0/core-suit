using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using CoreSuit.Domain.DTOs.FilterOptions;
using CoreSuit.Domain.DTOs.Paginate;
using CoreSuit.Domain.Interfaces.Repositories;
using CoreSuit.Domain.Requests;
using CoreSuit.Domain.Utils;
using CoreSuit.Infrastructure.Context;

namespace HubHuracan.Infrastructure.Repositories;

/// <summary>
/// Repositório base genérico para operações de persistência no banco de dados.
/// Oferece métodos reutilizáveis como adicionar, atualizar, excluir logicamente, buscar com filtros, ordenação e paginação.
/// Ideal para ser herdado por repositórios específicos de entidades.
/// </summary>
public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : class
{
    protected ApplicationDbContext Context { get; }
    protected DbSet<TEntity> DbSet { get; set; }

    public BaseRepository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }

    /// <summary>
    /// Salva as alterações pendentes no contexto e realiza o commit da transação atual, se houver.
    /// </summary>
    public virtual async Task Commit()
    {
        if (Context != null)
            await Context.SaveChangesAsync();

        if (Context != null && Context.Database != null && Context.Database.CurrentTransaction != null)
            await Context.Database.CurrentTransaction.CommitAsync();
    }

    /// <summary>
    /// Obtém a estratégia de execução atual, usada para implementar políticas de retry (ex: falhas transitórias).
    /// </summary>
    public virtual IExecutionStrategy GetStrategy()
    {
        IExecutionStrategy? strategy = Context.Database.CreateExecutionStrategy();
        if (strategy == null) throw new Exception("Falha ao obter strategy");
        return strategy;
    }

    /// <summary>
    /// Cancela todas as alterações em uma transação atual (rollback).
    /// </summary>
    public virtual async Task RollBack()
    {
        if (Context != null && Context.Database != null && Context.Database.CurrentTransaction != null)
            await Context.Database.CurrentTransaction.RollbackAsync();
    }

    /// <summary>
    /// Inicia uma nova transação no contexto do banco de dados.
    /// </summary>
    public virtual async Task<IDbContextTransaction> BeginTransaction()
    {
        return await Context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Adiciona uma nova entidade no banco e salva imediatamente.
    /// </summary>
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Marca uma entidade como deletada logicamente, atribuindo o valor atual em DeletedAt.
    /// </summary>
    public virtual async Task DeleteAsync(TEntity entity)
    {
        entity.GetType().GetProperty("DeletedAt")!.SetValue(entity, GetDateTimeBrasilia.Get());
        await UpdateAsync(entity);
    }

    /// <summary>
    /// Busca uma entidade pelo ID, com opção de ignorar registros logicamente deletados.
    /// </summary>
    public async Task<TEntity?> GetAsync(int id, bool checkDeletedAt = true)
    {
        var item = await DbSet.FindAsync(id);
        if (item == null) return null;

        if (checkDeletedAt)
        {
            var deletedAtProperty = item.GetType().GetProperty("DeletedAt");
            if (deletedAtProperty != null && deletedAtProperty.GetValue(item) != null)
                return null;
        }
        return item;
    }

    /// <summary>
    /// Busca uma entidade pelo ID e valida se pertence à filial informada (BranchId).
    /// </summary>
    public async Task<TEntity?> GetWithBranchAsync(int id, int branchId)
    {
        var item = await GetAsync(id);
        if (item != null && (int)item.GetType().GetProperty("BranchId")!.GetValue(item)! != branchId)
            return null;
        return item;
    }

    /// <summary>
    /// Adiciona uma lista de entidades ao banco.
    /// </summary>
    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null || !entities.Any())
            throw new ArgumentException("A coleção de entidades não pode ser nula ou vazia.", nameof(entities));

        await DbSet.AddRangeAsync(entities);
        await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove a entidade do controle de rastreamento do contexto.
    /// </summary>
    public virtual void Detach(TEntity entity)
    {
        Context.Entry(entity).State = EntityState.Detached;
    }

    /// <summary>
    /// Atualiza uma entidade no banco de dados.
    /// </summary>
    public virtual async Task UpdateAsync(TEntity entity)
    {
        var entry = Context.Entry(entity);
        DbSet.Attach(entity);
        entry.State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Retorna todos os registros com base nos filtros e ordenações definidos em <see cref="FilterOptionsDTO{TEntity}"/>.
    /// </summary>
    public virtual IQueryable<TEntity> All(FilterOptionsDTO<TEntity> options)
    {
        var query = options.ReadOnly ? DbSet.AsNoTracking() : DbSet.AsQueryable();

        ValidateColumnExists("DeletedAt");
        if (options.OnlyActives) ValidateColumnExists("Active");

        if (options.Where != null)
            query = query.Where(options.Where);

        if (!options.IncludeDeleted)
            query = AddNullFilter(query, "DeletedAt");

        if (options.OnlyActives)
            query = AddBoolFilter(query, "Active", true);

        query = ApplyOrdering(query, options.SortBy, options.Direction);
        return query;
    }

    /// <summary>
    /// Valida se a propriedade informada existe na entidade antes de aplicá-la dinamicamente.
    /// </summary>
    private void ValidateColumnExists(string columnName)
    {
        var property = typeof(TEntity).GetProperty(columnName);
        if (property == null)
            throw new ValidationException($"A coluna '{columnName}' não existe na entidade '{typeof(TEntity).Name}'.");
    }

    /// <summary>
    /// Retorna uma consulta com base em um filtro (expressão lambda).
    /// </summary>
    public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = false)
    {
        return @readonly ? DbSet.Where(predicate).AsNoTracking() : DbSet.Where(predicate);
    }

    private IQueryable<T> AddNullFilter<T>(IQueryable<T> query, string propertyName)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var propExpression = Expression.Property(param, propertyName);
        var filterLambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(propExpression, Expression.Constant(null)), param);
        return query.Where(filterLambda);
    }

    private IQueryable<T> AddBoolFilter<T>(IQueryable<T> query, string propertyName, bool value)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var propExpression = Expression.Property(param, propertyName);
        var filterLambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(propExpression, Expression.Constant(value)), param);
        return query.Where(filterLambda);
    }

    private IQueryable<T> AddIntFilter<T>(IQueryable<T> query, string propertyName, int value)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var propExpression = Expression.Property(param, propertyName);
        var filterLambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(propExpression, Expression.Constant(value)), param);
        return query.Where(filterLambda);
    }

    /// <summary>
    /// Aplica ordenação genérica (ascendente ou descendente) a uma consulta com base em uma ou mais propriedades.
    /// </summary>
    private IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string? sortBy = "Id", string? direction = "desc")
    {
        string methodName = direction == "desc" ? "OrderByDescending" : "OrderBy";
        Type entityType = typeof(T);
        string[] properties = sortBy!.Split('.');
        ParameterExpression parameter = Expression.Parameter(entityType, "x");
        Expression propertyAccess = parameter;

        foreach (string prop in properties)
            propertyAccess = Expression.PropertyOrField(propertyAccess, prop);

        LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);
        MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { entityType, propertyAccess.Type }, query.Expression, Expression.Quote(orderByExp));

        return query.Provider.CreateQuery<T>(resultExp);
    }

    /// <summary>
    /// Aplica paginação e ordenação combinadas de forma assíncrona.
    /// </summary>
    public async Task<PaginationDTO<T>> ApplyPaginationAndOrderingAsync<T>(IQueryable<T> query, PaginationOrderRequest request)
    {
        query = ApplyOrdering(query, request.SortBy, request.Direction);
        return await PaginateDataAsync(query, request.PageNumber, request.PageSize);
    }

    /// <summary>
    /// Realiza a paginação da consulta, retornando o conjunto de dados da página solicitada com total de registros.
    /// </summary>
    private async Task<PaginationDTO<T>> PaginateDataAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
    {
        int totalCount = await query.CountAsync();
        ICollection<T> data = await query.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        return new PaginationDTO<T>(data, pageNumber, pageSize, totalCount);
    }

    /// <summary>
    /// Busca o último registro com base em um campo de data (DateTime).
    /// </summary>
    public virtual async Task<TEntity?> GetLastAsync(Expression<Func<TEntity, DateTime>> orderBy)
    {
        return await DbSet.OrderByDescending(orderBy).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Busca uma entidade pelo ID sem aplicar includes (relacionamentos).
    /// Pode ser sobrescrito em repositórios específicos para incluir relacionamentos.
    /// </summary>
    public virtual async Task<TEntity?> GetByIdWithIncludesAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    #region Dispose

    /// <summary>
    /// Libera os recursos utilizados pelo contexto.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Executa o dispose de forma controlada.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;
        Context?.Dispose();
    }

    #endregion
}
