using System;
using System.Linq.Expressions;
using CoreSuit.Domain.DTOs.FilterOptions;
using CoreSuit.Domain.DTOs.Paginate;
using CoreSuit.Domain.Requests;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoreSuit.Domain.Interfaces.Repositories;

/// <summary>
/// Interface base genérica para repositórios. Define operações padrão para manipulação de entidades no banco de dados.
/// Ideal para aplicar o princípio DRY e reutilizar comportamentos comuns em múltiplos repositórios.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade a ser manipulada.</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Adiciona uma nova entidade no banco.
    /// </summary>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Adiciona uma coleção de entidades no banco.
    /// </summary>
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Atualiza uma entidade existente.
    /// </summary>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Exclui logicamente uma entidade, definindo a data de exclusão.
    /// </summary>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Busca uma entidade pelo ID.
    /// </summary>
    /// <param name="checkDeletedAt">Se verdadeiro, ignora registros com DeletedAt preenchido.</param>
    Task<TEntity?> GetAsync(int id, bool checkDeletedAt = true);

    /// <summary>
    /// Busca uma entidade por ID e valida se pertence à filial (BranchId) especificada.
    /// </summary>
    Task<TEntity?> GetWithBranchAsync(int id, int branchId);

    /// <summary>
    /// Retorna todos os registros com filtros e ordenação, de acordo com as opções fornecidas.
    /// </summary>
    IQueryable<TEntity> All(FilterOptionsDTO<TEntity> options);

    /// <summary>
    /// Retorna uma lista filtrada de registros com base em uma expressão lambda.
    /// </summary>
    /// <param name="readonly">Se verdadeiro, a consulta será realizada sem rastreamento (AsNoTracking).</param>
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, bool @readonly = false);

    /// <summary>
    /// Desanexa uma entidade do contexto, interrompendo o rastreamento dela.
    /// </summary>
    void Detach(TEntity entity);

    /// <summary>
    /// Salva as alterações pendentes no contexto.
    /// </summary>
    Task Commit();

    /// <summary>
    /// Obtém a estratégia de execução do banco de dados (útil para políticas de retry em falhas transitórias).
    /// </summary>
    IExecutionStrategy GetStrategy();

    /// <summary>
    /// Realiza rollback da transação atual.
    /// </summary>
    Task RollBack();

    /// <summary>
    /// Inicia uma nova transação explícita no banco de dados.
    /// </summary>
    Task<IDbContextTransaction> BeginTransaction();

    /// <summary>
    /// Aplica paginação e ordenação a uma consulta e retorna os dados paginados.
    /// </summary>
    Task<PaginationDTO<T>> ApplyPaginationAndOrderingAsync<T>(IQueryable<T> query, PaginationOrderRequest request);

    /// <summary>
    /// Retorna o último registro com base em um campo do tipo DateTime.
    /// </summary>
    Task<TEntity?> GetLastAsync(Expression<Func<TEntity, DateTime>> orderBy);
}
