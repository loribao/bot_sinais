using BotSinais.Infrastructure.Shared.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BotSinais.Infrastructure.Modules.Signals;

/// <summary>
/// Extensões de configuração para o módulo de Sinais de Trading
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona serviços de sinais de trading
    /// </summary>
    public static IServiceCollection AddSignalsModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração do MassTransit para eventos de sinais
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        services.AddMassTransitWithConfiguration(
            isDevelopment: isDevelopment,
            rabbitMqConnectionString: configuration.GetConnectionString("RabbitMQ"),
            assembliesWithHandlers: typeof(ServiceCollectionExtensions).Assembly
        );

        // Registra handlers de evento específicos de sinais
        services.AddDomainEventHandlers(typeof(ServiceCollectionExtensions).Assembly);

        // TODO: Adicionar repositórios e serviços específicos de sinais
        // services.AddScoped<ITradingSignalRepository, TradingSignalRepository>();
        // services.AddScoped<ISignalGenerationService, SignalGenerationService>();
        // services.AddScoped<IPortfolioService, PortfolioService>();

        return services;
    }
}
