using System;
using Swashbuckle.AspNetCore.Annotations;

namespace CoreSuit.Domain.Requests.Example;

public class ExampleRequest : PaginationOrderRequest
{
    [SwaggerSchema("Nome para filtro de busca (contém, insensível a maiúsculas e minúsculas)")]
    public string Name { get; set; } = string.Empty;

    [SwaggerSchema("Descrição para filtro de busca (contém, insensível a maiúsculas e minúsculas)")]
    public string Description { get; set; } = string.Empty;
}
