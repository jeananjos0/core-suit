using System;
using System.ComponentModel.DataAnnotations;

namespace CoreSuit.Domain.Constants;

/// <summary>
/// Classe utilitária que centraliza o acesso às variáveis de ambiente obrigatórias
/// utilizadas pela aplicação, especialmente para configuração de banco de dados.
/// Lança exceção caso alguma variável essencial não esteja definida.
/// </summary>
public static class EnvironmentVariables
{
    #region Database

    /// <summary>
    /// Nome do host do banco de dados.
    /// Exemplo: "localhost", "db", ou o nome do serviço no Docker Compose.
    /// </summary>
    public static readonly string DbHost = GetRequiredEnvironmentVariable("DB_HOST");

    /// <summary>
    /// Porta de conexão do banco de dados.
    /// Exemplo: "5432" para PostgreSQL.
    /// </summary>
    public static readonly string DbPort = GetRequiredEnvironmentVariable("DB_PORT");

    /// <summary>
    /// Nome de usuário para autenticação no banco de dados.
    /// </summary>
    public static readonly string DbUser = GetRequiredEnvironmentVariable("DB_USER");

    /// <summary>
    /// Senha do usuário do banco de dados.
    /// </summary>
    public static readonly string DbPassword = GetRequiredEnvironmentVariable("DB_PASSWORD");

    /// <summary>
    /// Nome da base de dados a ser utilizada.
    /// </summary>
    public static readonly string DbDatabase = GetRequiredEnvironmentVariable("DB_DATABASE");

    #endregion

    /// <summary>
    /// Obtém uma variável de ambiente obrigatória.
    /// Lança exceção caso a variável não esteja definida.
    /// </summary>
    /// <param name="key">Nome da variável de ambiente.</param>
    /// <returns>Valor da variável de ambiente.</returns>
    /// <exception cref="ValidationException">Lançada quando a variável não está definida no ambiente.</exception>
    private static string GetRequiredEnvironmentVariable(string key)
    {
        return Environment.GetEnvironmentVariable(key)
               ?? throw new ValidationException($"Environment variable {key} not found");
    }
}
