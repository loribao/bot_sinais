using BotSinais.Infrastructure.Modules.Auth;
using BotSinais.Infrastructure.Modules.DataManagement;
using BotSinais.Infrastructure.Modules.Signals;
using BotSinais.Infrastructure.Modules.Strategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BotSinais.Infrastructure.Shared;

/// <summary>
/// Extensões centralizadas que unificam todos os módulos do Bot Sinais
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona todos os módulos da infraestrutura do Bot Sinais
    /// Este é o ponto de entrada único para configuração de todos os serviços
    /// </summary>
    public static IServiceCollection AddBotSinaisInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurações básicas do ASP.NET Core
        services.AddProblemDetails();
        
        // Configuração de Controllers com opções JSON
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

        // OpenAPI/Swagger
        services.AddOpenApi();

        // === MÓDULOS DO BOT SINAIS ===
        
        // Módulo de Autenticação e Autorização
        services.AddAuthModule(configuration);
        
        // Módulo de Gerenciamento de Dados de Mercado
        services.AddDataManagementModule(configuration);
        
        // Módulo de Sinais de Trading
        services.AddSignalsModule(configuration);
        
        // Módulo de Estratégias de Trading
        services.AddStrategiesModule(configuration);

        return services;
    }

    /// <summary>
    /// Adiciona serviços específicos para projetos API
    /// </summary>
    public static IServiceCollection AddBotSinaisApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Serviços específicos da API podem ser adicionados aqui
        // Por exemplo: rate limiting, API versioning, etc.
        
        return services;
    }

    /// <summary>
    /// Adiciona serviços específicos para projetos Web (Blazor)
    /// </summary>
    public static IServiceCollection AddBotSinaisWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurações específicas do Blazor
        services.AddRazorPages();
        services.AddServerSideBlazor();
        
        return services;
    }
}
