namespace CoreSuit.Infrastructure.TablesDefinition;

/// <summary>
/// Contém definições centralizadas das tabelas utilizadas na aplicação,
/// incluindo o schema padrão e instâncias da classe <see cref="TableInfo"/>.
/// Essa abordagem facilita o uso padronizado de metadados em operações dinâmicas,
/// geração de scripts, auditoria ou documentação.
/// </summary>
public class Tables
{
    /// <summary>
    /// Define o schema padrão utilizado para todas as tabelas do sistema.
    /// Pode ser referenciado para evitar repetição de strings e garantir consistência.
    /// </summary>
    public static string DefaultSchema => "SC_001";

    /// <summary>
    /// Representa a definição da tabela de exemplo.
    /// Inclui o nome da tabela, o schema associado e uma breve descrição.
    /// </summary>
    public static readonly TableInfo ExampleTable = new("ExampleTable", DefaultSchema, "Tabela de exemplo.");
}
