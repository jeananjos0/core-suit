using Microsoft.AspNetCore.Mvc;
using CoreSuit.Application.Interfaces;
using CoreSuit.Domain.DTOs.Paginate;
using CoreSuit.Domain.Requests;
using Swashbuckle.AspNetCore.Annotations;

namespace CoreSuit.API.Controllers;

/// <summary>
/// Controller base genérico para manipulação de entidades via API REST.
/// Define endpoints padrão para operações CRUD (Create, Read, Update, Delete) e ativação.
/// Pode ser herdado por controllers específicos para reaproveitar a lógica genérica.
/// </summary>
/// <typeparam name="TEntity">Entidade do domínio (modelo do banco de dados).</typeparam>
/// <typeparam name="TDto">DTO de retorno usado para exibição.</typeparam>
/// <typeparam name="TCreateDto">DTO usado para criação de registros.</typeparam>
/// <typeparam name="TUpdateDto">DTO usado para atualização de registros.</typeparam>
[ApiController]
[Route("[controller]")]
public class BaseController<TEntity, TDto, TCreateDto, TUpdateDto> : ControllerBase
    where TEntity : class
{
    private readonly IBaseService<TEntity, TDto, TCreateDto, TUpdateDto> _service;

    /// <summary>
    /// Injeta o serviço base responsável pela lógica de negócio da entidade.
    /// </summary>
    public BaseController(IBaseService<TEntity, TDto, TCreateDto, TUpdateDto> service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todos os registros da entidade, com suporte a paginação e ordenação.
    /// </summary>
    /// <param name="request">Parâmetros de paginação e ordenação.</param>
    [SwaggerOperation(Summary = "Obter todos os registros", Description = "Retorna uma lista paginada de registros")]
    [HttpGet, ProducesResponseType(200)]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationOrderRequest request)
    {
        var result = await _service.GetAllAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Retorna os detalhes de um único registro com base no seu ID.
    /// </summary>
    /// <param name="id">Identificador do registro.</param>
    [SwaggerOperation(Summary = "Obter registro por ID", Description = "Retorna os detalhes de um registro pelo ID")]
    [HttpGet("{id}"), ProducesResponseType(200)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Cria um novo registro da entidade no banco de dados.
    /// </summary>
    /// <param name="request">DTO com os dados para criação.</param>
    [SwaggerOperation(Summary = "Criar um novo registro", Description = "Cria um novo registro no banco de dados")]
    [HttpPost, ProducesResponseType(201)]
    public async Task<IActionResult> CreateAsync([FromBody] TCreateDto request)
    {
        await _service.CreateAsync(request);
        return StatusCode(201);
    }

    /// <summary>
    /// Atualiza os dados de um registro existente.
    /// </summary>
    /// <param name="request">DTO com os dados atualizados e o ID do registro.</param>
    [SwaggerOperation(Summary = "Atualizar um registro", Description = "Edita os valores de um registro existente")]
    [HttpPut, ProducesResponseType(200)]
    public async Task<IActionResult> UpdateAsync([FromBody] TUpdateDto request)
    {
        await _service.UpdateAsync(request);
        return Ok();
    }

    /// <summary>
    /// Inativa (soft delete) um registro com base no ID.
    /// </summary>
    /// <param name="id">Identificador do registro.</param>
    [SwaggerOperation(Summary = "Deletar um registro", Description = "Marca o registro como deletado")]
    [HttpDelete("{id}"), ProducesResponseType(200)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }

    /// <summary>
    /// Reativa um registro que foi previamente marcado como deletado (soft delete).
    /// </summary>
    /// <param name="id">Identificador do registro a ser reativado.</param>
    [SwaggerOperation(Summary = "Ativar um registro", Description = "Reativa um registro que foi desativado")]
    [HttpPatch("{id}"), ProducesResponseType(200)]
    public async Task<IActionResult> ActiveAsync(int id)
    {
        await _service.ActiveAsync(id);
        return Ok();
    }
}
