using System;
using CoreSuit.Domain.Utils;

namespace CoreSuit.Domain.Entities;

/// <summary>
/// Classe base para entidades da aplicação.
/// Contém propriedades comuns de controle como ID, data de criação, atualização e exclusão lógica.
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Identificador único da entidade (chave primária).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Data e hora em que o registro foi criado.
    /// Inicializada automaticamente com o horário de Brasília.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Data e hora da última atualização do registro.
    /// Inicializada automaticamente com o horário de Brasília.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Data e hora em que o registro foi logicamente excluído (soft delete).
    /// Caso esteja preenchido, o registro é considerado como inativo.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Construtor que inicializa as propriedades de data com o fuso horário de Brasília.
    /// </summary>
    public BaseEntity()
    {
        CreatedAt = GetDateTimeBrasilia.Get();
        UpdatedAt = GetDateTimeBrasilia.Get();
    }
}
