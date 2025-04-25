using System;

namespace CoreSuit.Domain.Entities;

/// <summary>
/// Entidade de exemplo utilizada para testes, demonstrações ou validação de funcionalidades.
/// Herda propriedades padrão como Id, CreatedAt, UpdatedAt e DeletedAt da <see cref="BaseEntity"/>.
/// </summary>
public class ExampleEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
