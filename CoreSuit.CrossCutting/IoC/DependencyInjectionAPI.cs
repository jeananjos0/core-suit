using System;
using HubHuracan.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoreSuit.Application.DTOs;
using CoreSuit.Application.Interfaces;
using CoreSuit.Application.Services;
using CoreSuit.Domain.Constants;
using CoreSuit.Domain.Interfaces.Repositories;
using CoreSuit.Infrastructure.Context;
using CoreSuit.Infrastructure.Repositories;
using CoreSuit.Infrastructure.TablesDefinition;

namespace CoreSuit.CrossCutting.IoC;

/// <summary>
/// Classe responsável pela configuração da injeção de dependência de toda a aplicação.
/// Centraliza o registro de serviços, repositórios, contexto do banco e AutoMapper.
/// </summary>
public static class DependencyInjectionAPI
{
    /// <summary>
    /// Método de extensão que realiza o registro de todas as dependências da aplicação.
    /// </summary>
    /// <param name="services">Coleção de serviços utilizada pelo .NET Core.</param>
    /// <returns>Objeto IServiceCollection com os serviços registrados.</returns>
    public static IServiceCollection ServiceInjection(this IServiceCollection services)
    {
        ConfigureDatabase(services);
        RegisterApplicationServices(services);
        RegisterRepositories(services);
        ConfigureAutoMapper(services);

        return services;
    }

    /// <summary>
    /// Configura a conexão com o banco de dados PostgreSQL utilizando o Entity Framework Core.
    /// As variáveis de ambiente são lidas da classe <see cref="EnvironmentVariables"/>.
    /// Inclui configuração de assembly de migrações e política de retry.
    /// </summary>
    private static void ConfigureDatabase(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(@$"Host={EnvironmentVariables.DbHost};
                                    Port={EnvironmentVariables.DbPort};
                                    Username={EnvironmentVariables.DbUser};
                                    Password={EnvironmentVariables.DbPassword};
                                    Database={EnvironmentVariables.DbDatabase}",
            npgsqlOptions =>
            {
                // Define o assembly de migração
                npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);

                // Define a tabela de histórico de migrações personalizada
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Tables.DefaultSchema);

                // Ativa a política de retry para falhas transitórias
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null
                );

                // Exibe dados sensíveis nos logs (somente em desenvolvimento)
                options.EnableSensitiveDataLogging(true);
            });
        });
    }

    /// <summary>
    /// Registra os serviços de aplicação (camada Application).
    /// Isso inclui serviços genéricos e específicos como <see cref="ExampleService"/>.
    /// </summary>
    private static void RegisterApplicationServices(IServiceCollection services)
    {
        // Registro do serviço base genérico
        services.AddScoped(typeof(IBaseService<,,,>), typeof(BaseService<,,,>));

        // Registro de serviços específicos
        services.AddScoped<IExampleService, ExampleService>();
    }

    /// <summary>
    /// Registra os repositórios da camada de infraestrutura.
    /// Isso inclui o repositório base genérico e repositórios específicos como <see cref="ExampleRepository"/>.
    /// </summary>
    private static void RegisterRepositories(IServiceCollection services)
    {
        // Registro do repositório base genérico
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        // Registro de repositórios específicos
        services.AddScoped<IExampleRepository, ExampleRepository>();
    }

    /// <summary>
    /// Configura o AutoMapper, registrando os perfis de mapeamento entre entidades e DTOs.
    /// </summary>
    private static void ConfigureAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
    }
}
