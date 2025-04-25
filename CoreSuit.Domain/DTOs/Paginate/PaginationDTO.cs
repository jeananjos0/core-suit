using System;

namespace CoreSuit.Domain.DTOs.Paginate;

/// <summary>
/// DTO utilizado para representar uma resposta paginada de uma consulta.
/// Contém informações sobre os dados retornados, página atual, total de páginas e quantidade total de registros.
/// </summary>
/// <typeparam name="T">Tipo dos dados contidos na lista paginada.</typeparam>
public class PaginationDTO<T>
{
    /// <summary>
    /// Coleção de dados da página atual.
    /// </summary>
    public ICollection<T> Data { get; set; }

    /// <summary>
    /// Número da página atual (iniciando geralmente em 0 ou 1, conforme implementação).
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Quantidade total de páginas disponíveis, baseado no total de registros e no tamanho da página.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Quantidade de registros por página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Quantidade total de registros disponíveis na base de dados.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Construtor que calcula automaticamente o número total de páginas com base no total de registros e tamanho da página.
    /// </summary>
    /// <param name="data">Lista de dados da página atual.</param>
    /// <param name="currentPage">Número da página atual.</param>
    /// <param name="pageSize">Quantidade de itens por página.</param>
    /// <param name="totalCount">Total de itens disponíveis na consulta.</param>
    public PaginationDTO(ICollection<T> data, int currentPage, int pageSize, int totalCount)
    {
        Data = data;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;

        // Calcula o número total de páginas com base no total de registros e tamanho da página
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
