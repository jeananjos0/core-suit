using System;

namespace CoreSuit.Domain.Utils;

/// <summary>
/// Classe utilitária responsável por obter a data e hora atual no fuso horário de Brasília (America/Sao_Paulo).
/// Útil para sistemas que precisam manter consistência temporal com o horário oficial do Brasil,
/// independentemente da localização do servidor (UTC, cloud, etc).
/// </summary>
public class GetDateTimeBrasilia
{
    /// <summary>
    /// Retorna a data e hora atual convertida para o fuso horário de Brasília.
    /// A propriedade <see cref="DateTimeKind.Unspecified"/> é usada para evitar conflitos
    /// ao salvar no banco de dados, permitindo que o provider assuma o controle do fuso horário, se necessário.
    /// </summary>
    /// <returns>Data e hora atual no fuso horário de Brasília.</returns>
    public static DateTime Get()
    {
        // Obtém a data e hora atual no formato UTC
        DateTime dateTimeUtc = DateTime.UtcNow;

        // Define o fuso horário de Brasília
        TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        // Converte a data e hora UTC para o fuso horário de Brasília
        DateTime brasiliaDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, brasiliaTimeZone);

        // Retorna o DateTime de Brasília com DateTimeKind.Unspecified
        // Isso evita conflitos com bancos de dados que armazenam como UTC ou local
        DateTime resultDateTime = DateTime.SpecifyKind(brasiliaDateTime, DateTimeKind.Unspecified);

        return resultDateTime;
    }
}
