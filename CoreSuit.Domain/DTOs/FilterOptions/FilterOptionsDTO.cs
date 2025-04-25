using System;
using System.Linq.Expressions;

namespace CoreSuit.Domain.DTOs.FilterOptions;

/// <summary>
/// DTO utilizado para fornecer opções dinâmicas de filtragem, ordenação e controle de leitura em consultas genéricas.
/// Pode ser usado com repositórios genéricos para aplicar filtros de forma flexível sem duplicar lógica.
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade a ser consultada.</typeparam>
public class FilterOptionsDTO<TEntity> where TEntity : class
{
    /// <summary>
    /// Define se a consulta será feita em modo somente leitura (sem tracking).
    /// Padrão: true.
    /// </summary>
    public bool ReadOnly { get; set; } = true;

    /// <summary>
    /// Define se a consulta deve retornar apenas os registros marcados como ativos.
    /// Padrão: false.
    /// </summary>
    public bool OnlyActives { get; set; } = false;

    /// <summary>
    /// Expressão lambda usada para aplicar uma condição personalizada de filtro na consulta.
    /// </summary>
    public Expression<Func<TEntity, bool>>? Where { get; set; }

    /// <summary>
    /// Define se registros logicamente deletados (DeletedAt ≠ null) devem ser incluídos na consulta.
    /// Padrão: false (ou seja, registros deletados são ignorados).
    /// </summary>
    public bool IncludeDeleted { get; set; } = false;

    /// <summary>
    /// Nome da propriedade pela qual os resultados devem ser ordenados.
    /// Padrão: \"Id\".
    /// </summary>
    public string? SortBy { get; set; } = "Id";

    /// <summary>
    /// Direção da ordenação: \"asc\" para ascendente ou \"desc\" para descendente.
    /// Padrão: \"desc\".
    /// </summary>
    public string? Direction { get; set; } = "desc";
}
