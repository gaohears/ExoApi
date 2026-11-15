using System.Text.Json;
using System.Text.Json.Serialization;
using ExoApi.Exceptions;
using ExoApi.Models;

namespace ExoApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NaoEncontradoException ex)
        {
            await EscreverAsync(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (RegraDeNegocioException ex)
        {
            await EscreverAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            // Erro inesperado: o detalhe técnico vai só para o log, nunca para o cliente
            _logger.LogError(ex, "Erro inesperado ao processar a requisição.");
            await EscreverAsync(context, StatusCodes.Status500InternalServerError,
                "Ocorreu um erro interno no servidor. Tente novamente mais tarde.");
        }
    }

    public static Task EscreverAsync(HttpContext context, int status, string mensagem)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json; charset=utf-8";

        var json = JsonSerializer.Serialize(
            new ErroResposta(status, mensagem),
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

        return context.Response.WriteAsync(json);
    }
}