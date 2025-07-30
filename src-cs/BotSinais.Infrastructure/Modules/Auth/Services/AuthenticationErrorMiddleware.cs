using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BotSinais.Infrastructure.Modules.Auth.Services;

/// <summary>
/// Middleware para tratamento customizado de erros de autenticação e autorização
/// </summary>
public class AuthenticationErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationErrorMiddleware> _logger;

    public AuthenticationErrorMiddleware(RequestDelegate next, ILogger<AuthenticationErrorMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o processamento da requisição");
            await HandleExceptionAsync(context, ex);
            return;
        }

        // Tratar respostas de erro de autenticação/autorização
        if (context.Response.StatusCode == 401 && !context.Response.HasStarted)
        {
            await HandleUnauthorizedAsync(context);
        }
        else if (context.Response.StatusCode == 403 && !context.Response.HasStarted)
        {
            await HandleForbiddenAsync(context);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var response = new
        {
            Error = "Erro interno do servidor",
            Message = "Ocorreu um erro inesperado durante o processamento da requisição.",
            Timestamp = DateTime.UtcNow,
            TraceId = context.TraceIdentifier
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private async Task HandleUnauthorizedAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            Error = "Não autorizado",
            Message = "Token de acesso necessário. Inclua o header: Authorization: Bearer {seu-jwt-token}",
            Timestamp = DateTime.UtcNow,
            LoginUrl = "http://localhost:8080/realms/master/protocol/openid-connect/auth",
            TokenUrl = "http://localhost:8080/realms/master/protocol/openid-connect/token"
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private async Task HandleForbiddenAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            Error = "Acesso negado",
            Message = "Você não possui permissão para acessar este recurso.",
            Timestamp = DateTime.UtcNow,
            RequiredRoles = "Verifique se possui as permissões necessárias no Keycloak"
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
