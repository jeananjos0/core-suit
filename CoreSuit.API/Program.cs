using dotenv.net;
using CoreSuit.API;
using CoreSuit.API.Middlewares;
using CoreSuit.CrossCutting.IoC;
using CoreSuit.Infrastructure.Context;
using System.Globalization;

var cultureInfo = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Carrega variáveis de ambiente definidas no arquivo .env
/// (útil para manter senhas, strings de conexão e configurações separadas do código fonte).
/// </summary>
DotEnv.Load();

/// <summary>
/// Configura todos os serviços que a aplicação vai utilizar, como Controllers, Swagger, CORS e injeções.
/// </summary>
ConfigureServices(builder.Services);

var app = builder.Build();

/// <summary>
/// Executa as migrações pendentes no banco de dados assim que o sistema é iniciado.
/// Essa etapa garante que o schema esteja sempre atualizado.
/// </summary>
ApplicationDbContextMigrator.Migrate(app);

/// <summary>
/// Configura o pipeline de requisições HTTP, middlewares globais, CORS e documentação Swagger.
/// </summary>
ConfigureMiddleware(app);

/// <summary>
/// Inicia a execução do servidor da aplicação ASP.NET Core.
/// </summary>
app.Run();

/// <summary>
/// Método responsável por registrar os serviços da aplicação na injeção de dependência.
/// </summary>
void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    // Registro da camada de injeção de dependência (IoC)
    services.ServiceInjection();

    // Configuração e documentação da API via Swagger
    services.AddSwaggerConfiguration();

    // Configuração de CORS permitindo qualquer origem
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAny", policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader();
        });
    });

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(); // Necessário para expor a documentação via Swagger UI
}

/// <summary>
/// Define os middlewares que fazem parte do pipeline da aplicação, incluindo tratamento global de erros,
/// permissões de origem (CORS), autenticação e roteamento.
/// </summary>
void ConfigureMiddleware(WebApplication app)
{
    app.UseCors("AllowAny");

    // Middleware global para tratamento de exceções com resposta padrão
    app.UseMiddleware<HandleErrorMiddleware>();

    // ==================================================================
    // OPCIONAL: Define um PathBase caso a aplicação esteja em uma subpasta
    // Exemplo: www.dominio.com/CoreSuit
    // Descomente esse bloco se sua aplicação precisar funcionar com subdiretórios
    // ==================================================================
    // if (!app.Environment.IsDevelopment())
    // {
    //     app.Use((context, next) =>
    //     {
    //         context.Request.PathBase = "/CoreSuit";
    //         return next();
    //     });
    // }

    // Configuração do Swagger para visualização da documentação da API
    app.UseSwaggerConfiguration(app.Environment);

    // Middleware para controle de autorização
    app.UseAuthorization();

    // Mapeia automaticamente todos os endpoints dos controllers
    app.MapControllers();
}
