using System;

namespace CoreSuit.Application.DTOs.Example;

/// <summary>
/// DTO utilizado para retornar dados completos da entidade Example, incluindo informações de auditoria como datas de criação, atualização e exclusão.
/// 
/// Herda de <see cref="UpdateExampleDTO"/> para reaproveitar os campos de identificação (Id) e entrada (Name, Description),
/// evitando duplicação de código e garantindo consistência entre os dados usados em atualização e os dados retornados.
/// </summary>
public class ExampleDTO : UpdateExampleDTO
{
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO utilizado para criação de novos registros da entidade Example.
/// 
/// Contém apenas os campos essenciais que devem ser fornecidos pelo usuário no momento da criação,
/// sem incluir o Id, pois ele será gerado automaticamente pelo sistema.
/// </summary>
public class CreateExampleDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// DTO utilizado para atualização de registros existentes da entidade Example.
/// 
/// Herda de <see cref="CreateExampleDTO"/> para reaproveitar os campos de entrada (Name, Description),
/// adicionando o campo Id necessário para identificar o registro a ser atualizado.
/// </summary>
public class UpdateExampleDTO : CreateExampleDTO
{
    public int Id { get; set; }
}
