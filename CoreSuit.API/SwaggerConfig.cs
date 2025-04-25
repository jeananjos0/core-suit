using Microsoft.OpenApi.Models;

namespace CoreSuit.API;

/// <summary>
/// Classe responsável por configurar o Swagger (documentação interativa da API).
/// Este exemplo representa a estrutura de um sistema base desenvolvido com padrões de Clean Architecture,
/// pronto para ser expandido em qualquer outro sistema, com autenticação JWT e organização em camadas.
/// </summary>
public static class SwaggerConfig
{
    /// <summary>
    /// Adiciona e configura o Swagger ao container de serviços da aplicação.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Documento Swagger principal com título e descrição detalhada sobre o sistema base
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Sistema Base - Clean Architecture",
                Version = "v1",
                Description = "Este sistema base foi desenvolvido como um exemplo completo e escalável, seguindo os princípios da Clean Architecture.\n\n" +
                              "Ele fornece a estrutura essencial para criar qualquer outro sistema de forma organizada, com as seguintes características:\n" +
                              "- ASP.NET Core com Entity Framework Core e PostgreSQL\n" +
                              "- Injeção de dependência com IoC\n" +
                              "- Separação em camadas: Application, Domain, Infrastructure e API\n" +
                              "- Suporte a Swagger com autenticação JWT (Bearer Token)\n" +
                              "- CRUD genérico com Soft Delete e Ativação\n" +
                              "- Padrões de DTOs, Services e Controllers reutilizáveis\n" +
                              "- Extensível para qualquer domínio de negócio"
            });

            // Configuração de autenticação JWT (Bearer Token)
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insira o token JWT no formato: Bearer {seu_token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Habilita suporte às anotações Swagger nos controllers
            c.EnableAnnotations();

#if !DEBUG
            // Adiciona base path padrão para ambientes fora do desenvolvimento
            c.AddServer(new OpenApiServer
            {
                Url = "/"
            });
#endif
        });
    }

    /// <summary>
    /// Habilita o Swagger e configura a interface Swagger UI.
    /// </summary>
    /// <param name="app">Instância da aplicação.</param>
    /// <param name="env">Informações do ambiente de execução.</param>
    public static void UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();

#if DEBUG
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Base - Clean Architecture");
            c.ConfigObject.AdditionalItems.Add("persistAuthorization", true);
        });
#else
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Base - Clean Architecture");
            c.RoutePrefix = "swagger";
            c.ConfigObject.AdditionalItems.Add("persistAuthorization", true);
        });
#endif
    }
}