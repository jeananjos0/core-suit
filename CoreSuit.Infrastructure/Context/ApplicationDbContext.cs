using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CoreSuit.Domain.Entities;
using CoreSuit.Infrastructure.TablesDefinition;

namespace CoreSuit.Infrastructure.Context;

/// <summary>
/// Representa o contexto do Entity Framework para a aplicação.
/// Responsável por mapear as entidades para as tabelas do banco de dados
/// e configurar comportamentos globais.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Construtor padrão que recebe as opções de configuração do contexto.
    /// </summary>
    /// <param name="options">Opções do DbContext (geralmente configuradas via injeção de dependência).</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    /// <summary>
    /// DbSet que representa a tabela Example no banco de dados.
    /// Utilizada para operações CRUD com a entidade ExampleEntity.
    /// </summary>
    public DbSet<ExampleEntity> Example { get; set; }

    /// <summary>
    /// Configurações adicionais aplicadas ao modelo durante a criação do contexto.
    /// Define o schema padrão, aplica configurações customizadas via Fluent API
    /// e padroniza o tipo de dados das propriedades DateTime.
    /// </summary>
    /// <param name="modelBuilder">Construtor de modelo do EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica configurações específicas mapeadas via Fluent API no projeto
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Define o schema padrão para todas as tabelas do contexto
        modelBuilder.HasDefaultSchema(Tables.DefaultSchema);

        // Converte todos os campos DateTime e DateTime? para o tipo PostgreSQL "timestamp without time zone"
        foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
        {
            IEnumerable<PropertyInfo>? properties = entityType.ClrType
                .GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

            foreach (PropertyInfo property in properties)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property(property.Name)
                    .HasColumnType("timestamp without time zone");
            }
        }
    }
}
