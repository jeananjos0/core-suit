using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreSuit.Infrastructure.Context;

/// <summary>
/// Responsável por aplicar automaticamente as migrações pendentes do Entity Framework
/// no momento da inicialização da aplicação.
/// </summary>
public class ApplicationDbContextMigrator
{
    /// <summary>
    /// Aplica todas as migrações pendentes no banco de dados.
    /// Esse método deve ser chamado no início da aplicação, geralmente dentro do Program.cs.
    /// </summary>
    /// <param name="applicationBuilder">Construtor da aplicação, usado para acessar os serviços registrados.</param>
    public static void Migrate(IApplicationBuilder applicationBuilder)
    {
        // Cria um escopo de serviço para acessar o DbContext
        using var serviceScoped = applicationBuilder.ApplicationServices.CreateScope();

        // Obtém a instância do ApplicationDbContext a partir do provedor de serviços
        ApplicationDbContext dbConbtext = serviceScoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Aplica todas as migrações pendentes no banco de dados
        dbConbtext.Database.Migrate();
    }
}
