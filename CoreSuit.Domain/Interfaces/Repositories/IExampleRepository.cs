using System;
using CoreSuit.Domain.Entities;
using CoreSuit.Domain.Requests.Example;

namespace CoreSuit.Domain.Interfaces.Repositories;

/// <summary>
/// Interface de repositório especializada para a entidade <see cref="ExampleEntity"/>.
/// Estende o repositório genérico <see cref="IBaseRepository{TEntity}"/> com métodos específicos para consultas com filtros personalizados.
/// </summary>
public interface IExampleRepository : IBaseRepository<ExampleEntity>
{
    /// <summary>
    /// Retorna uma lista de registros da entidade ExampleEntity aplicando os filtros fornecidos na requisição.
    /// Ideal para buscas dinâmicas baseadas em parâmetros como nome, descrição, paginação, ordenação, etc.
    /// </summary>
    /// <param name="request">Parâmetros de busca e filtro da requisição.</param>
    /// <returns>Consulta <see cref="IQueryable{ExampleEntity}"/> com os dados filtrados.</returns>
    IQueryable<ExampleEntity> GetAllAsync(ExampleRequest request);
}
