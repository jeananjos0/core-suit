using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace CoreSuit.API.Middlewares;

/// <summary>
/// DTO de resposta de erro padronizada para a API.
/// Utilizado para retornar mensagens de erro amigáveis ao cliente, incluindo tipo do erro e se ele é tratado.
/// </summary>
public class ErrorResponseDTO
{
    /// <summary>
    /// Indica se o erro foi previsto/tratado (como validações, argumentos inválidos, etc).
    /// </summary>
    public bool IsCustomException { get; set; }

    /// <summary>
    /// Lista de mensagens de erro a serem exibidas para o cliente.
    /// </summary>
    public List<string>? Messages { get; set; }

    /// <summary>
    /// Tipo da exceção capturada (ex: ValidationException, ArgumentException).
    /// </summary>
    public string? ErrorType { get; set; }
}

/// <summary>
/// Middleware responsável por capturar e tratar exceções globalmente na aplicação.
/// Centraliza o tratamento de erros e formata uma resposta padronizada para o cliente.
/// </summary>
public class HandleErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HandleErrorMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Construtor do middleware que injeta o pipeline da requisição, logger e provider.
    /// </summary>
    public HandleErrorMiddleware(RequestDelegate next, ILogger<HandleErrorMiddleware> logger, IServiceProvider serviceProvider)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Método que intercepta a execução da pipeline HTTP, captura exceções e retorna uma resposta customizada.
    /// </summary>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Continua a execução normal
        }
        catch (Exception error)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";

            string fullErrorMessage = GetFullErrorMessage(error);

            var responseError = new ErrorResponseDTO
            {
                ErrorType = error.GetType().Name
            };

            // Tratamento baseado no tipo de exceção
            switch (error)
            {
                case KeyNotFoundException e:
                    _logger.LogError(e, "Key not found");
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ValidationException e:
                    _logger.LogError(e, "Dados inválidos");
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseError.IsCustomException = true;
                    responseError.Messages = new List<string> { e.Message };
                    break;

                case InvalidOperationException e:
                    _logger.LogError(e, "Operação inválida");
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    responseError.IsCustomException = true;
                    responseError.Messages = new List<string> { e.Message };
                    break;

                case UnauthorizedAccessException e:
                    _logger.LogError(e, "Acesso negado");
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;

                case ArgumentException e:
                    _logger.LogError(e, "Argumento inválido");
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;

                default:
                    _logger.LogError(error, "Erro inesperado");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Se não for uma exceção customizada, exibe a mensagem padrão
            if (!responseError.IsCustomException)
                responseError.Messages = new List<string> { error.Message };

            // Serializa a resposta para JSON e escreve no corpo da resposta
            string result = JsonSerializer.Serialize(responseError);
            await response.WriteAsync(result);
        }
    }

    /// <summary>
    /// Método auxiliar que concatena todas as mensagens da exceção e suas InnerExceptions.
    /// </summary>
    private string GetFullErrorMessage(Exception ex)
    {
        var messages = new List<string>();
        while (ex != null)
        {
            messages.Add(ex.Message);
            ex = ex.InnerException;
        }
        return string.Join(" | ", messages);
    }
}
