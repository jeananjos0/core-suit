namespace CoreSuit.Infrastructure.TablesDefinition;

/// <summary>
/// Representa informações básicas sobre uma tabela do banco de dados,
/// incluindo seu nome, schema e uma descrição funcional.
/// </summary>
public class TableInfo
{
    /// <summary>
    /// Nome da tabela no banco de dados.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Schema (esquema) ao qual a tabela pertence. 
    /// Exemplo: "public", "dbo", etc.
    /// </summary>
    public string Schema { get; }

    /// <summary>
    /// Descrição funcional da tabela.
    /// Pode ser usada para fins de documentação ou geração automática de metadados.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="TableInfo"/>.
    /// </summary>
    /// <param name="name">Nome da tabela.</param>
    /// <param name="schema">Schema da tabela.</param>
    /// <param name="description">Descrição da tabela.</param>
    public TableInfo(string name, string schema, string description)
    {
        Name = name;
        Schema = schema;
        Description = description;
    }
}
