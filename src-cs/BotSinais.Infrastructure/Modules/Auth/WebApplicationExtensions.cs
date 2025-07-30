using BotSinais.Infrastructure.Modules.Auth.Services;
using Microsoft.AspNetCore.Builder;

namespace BotSinais.Infrastructure.Modules.Auth;

/// <summary>
/// Extensões de configuração de pipeline para o módulo de Autenticação
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configura pipeline de autenticação
    /// </summary>
    public static WebApplication UseAuthModule(this WebApplication app)
    {
        // Sessões (deve vir antes da autenticação)
        app.UseSession();

        // Middleware personalizado de autenticação
        app.UseAuthenticationErrorHandling();

        // Autenticação e autorização
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
