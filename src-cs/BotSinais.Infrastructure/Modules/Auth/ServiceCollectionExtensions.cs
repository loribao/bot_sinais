using BotSinais.Infrastructure.Modules.Auth.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BotSinais.Infrastructure.Modules.Auth;

/// <summary>
/// Extensões de configuração para o módulo de Autenticação
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona serviços de autenticação e autorização
    /// </summary>
    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração de autenticação com Keycloak
        services.AddAuthKeycloak(configuration);

        // Configuração de sessões (necessário para fluxo OAuth)
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.Name = "BotSinais.Session";
            options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
        });

        // TODO: Adicionar outros serviços específicos de autenticação
        // services.AddScoped<IUserService, UserService>();
        // services.AddScoped<IRoleService, RoleService>();

        return services;
    }
}
