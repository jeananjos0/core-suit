using System;

namespace CoreSuit.Domain.Requests;

/// <summary>
/// Classe base utilizada para requisições que suportam paginação e ordenação.
/// Permite limitar a quantidade de registros por página, navegar entre páginas,
/// e definir o campo e a direção da ordenação.
/// </summary>
public class PaginationOrderRequest
{
    private const int maxPageSize = 50;
    private const int minPageNumber = 0;

    private int _pageNumber = 0;
    private int _pageSize = 10;
    private string _sortBy;
    private string _direction;

    /// <summary>
    /// Construtor padrão. Define os valores padrão para ordenação: SortBy = \"Id\", Direction = \"desc\".
    /// </summary>
    public PaginationOrderRequest()
    {
        _sortBy = "Id";
        _direction = "desc";
    }

    /// <summary>
    /// Construtor que permite definir valores padrão para ordenação.
    /// </summary>
    /// <param name="defaultSortBy">Campo padrão para ordenação.</param>
    /// <param name="defaultDirection">Direção padrão da ordenação (\"asc\" ou \"desc\").</param>
    public PaginationOrderRequest(string defaultSortBy, string defaultDirection)
    {
        _sortBy = defaultSortBy;
        _direction = defaultDirection;
    }

    /// <summary>
    /// Número da página a ser retornada. O valor mínimo é 0 (primeira página).
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < minPageNumber ? minPageNumber : value;
    }

    /// <summary>
    /// Quantidade de registros por página. O valor máximo é 50.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > maxPageSize ? maxPageSize : value;
    }

    /// <summary>
    /// Campo pelo qual os registros devem ser ordenados. Valor padrão é \"Id\".
    /// </summary>
    public string SortBy
    {
        get => _sortBy;
        set => _sortBy = value ?? _sortBy;
    }

    /// <summary>
    /// Direção da ordenação: \"asc\" para ascendente, \"desc\" para descendente. Valor padrão é \"desc\".
    /// </summary>
    public string Direction
    {
        get => _direction;
        set => _direction = value ?? _direction;
    }
}
