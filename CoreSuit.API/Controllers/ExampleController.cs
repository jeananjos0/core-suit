using System;
using CoreSuit.Application.DTOs.Example;
using CoreSuit.Application.Interfaces;
using CoreSuit.Domain.Entities;

namespace CoreSuit.API.Controllers;

/// <summary>
/// Controller específico para a entidade <see cref="ExampleEntity"/>.
/// Herda toda a funcionalidade genérica do <see cref="BaseController{TEntity, TDto, TCreateDto, TUpdateDto}"/>,
/// incluindo os endpoints padrão de CRUD e ativação/desativação.
/// 
/// Este controller pode ser estendido com endpoints personalizados que sejam específicos da entidade Example,
/// como filtragens avançadas, ações específicas de negócio ou relatórios.
/// </summary>
/// <example>
/// Exemplo de rota adicional que pode ser adicionada neste controller:
/// 
/// <code>
/// [HttpGet(\"filter-by-name\")]
/// public async Task<IActionResult> FilterByName([FromQuery] string name)
/// {
///     var result = await _exampleService.FilterByNameAsync(name);
///     return Ok(result);
/// }
/// </code>
/// </example>
public class ExampleController : BaseController<ExampleEntity, ExampleDTO, CreateExampleDTO, UpdateExampleDTO>
{
    /// <summary>
    /// Construtor do controller de Example.
    /// Injeta o serviço específico <see cref="IExampleService"/> e repassa para o controller base.
    /// </summary>
    public ExampleController(IExampleService service) : base(service)
    {
    }

    // Aqui você pode adicionar métodos específicos para esta entidade, como por exemplo:
    // [HttpGet("ativos")]
    // public async Task<IActionResult> GetAtivos()
    // {
    //     var result = await _exampleService.ObterSomenteAtivos();
    //     return Ok(result);
    // }
}
