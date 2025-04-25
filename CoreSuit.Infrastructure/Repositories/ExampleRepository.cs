using System;
using CoreSuit.Domain.DTOs.FilterOptions;
using CoreSuit.Domain.Entities;
using CoreSuit.Domain.Interfaces.Repositories;
using CoreSuit.Domain.Requests.Example;
using CoreSuit.Infrastructure.Context;
using HubHuracan.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreSuit.Infrastructure.Repositories;

/// <summary>
/// Repositório responsável pelas operações de leitura de dados da entidade ExampleEntity.
/// </summary>
public class ExampleRepository : BaseRepository<ExampleEntity>, IExampleRepository
{
    public ExampleRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Retorna uma lista filtrada de ExampleEntity com suporte a busca por nome e descrição.
    /// Os filtros são aplicados utilizando comparação case-insensitive (ILike).
    /// </summary>
    /// <param name="request">Parâmetros de busca, como Name e Description.</param>
    /// <returns>IQueryable com os dados filtrados.</returns>
    /// <example>
    /// Requisição:
    /// <code>
    /// GET /api/example?name=hidráulico&amp;description=manutenção
    /// </code>
    /// </example>
    public IQueryable<ExampleEntity> GetAllAsync(ExampleRequest request)
    {
        IQueryable<ExampleEntity> data = base.All(new FilterOptionsDTO<ExampleEntity>());

        if (!string.IsNullOrWhiteSpace(request.Name))
            data = data.Where(x => EF.Functions.ILike(x.Name, $"%{request.Name}%"));

        if (!string.IsNullOrWhiteSpace(request.Description))
            data = data.Where(x => EF.Functions.ILike(x.Description, $"%{request.Description}%"));

        return data;
    }

    /*
     * Exemplo de sobrescrita de GetById com includes (se necessário carregar dados relacionados)
     * 
     * public override async Task<ExampleEntity?> GetByIdWithIncludesAsync(int id)
     * {
     *     return await DbSet
     *         .Include(d => d.Categoria)
     *         .Include(d => d.TipoProduto)
     *         .Include(d => d.Subcategoria)
     *         .Include(d => d.UnidadeDeMedida)
     *         .FirstOrDefaultAsync(d => d.Id == id);
     * }
     */
}
