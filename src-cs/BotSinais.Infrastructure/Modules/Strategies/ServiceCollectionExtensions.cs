using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BotSinais.Infrastructure.Modules.Strategies;

/// <summary>
/// Extensões de configuração para o módulo de Estratégias
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona serviços de estratégias de trading
    /// </summary>
    public static IServiceCollection AddStrategiesModule(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Adicionar repositórios e serviços específicos de estratégias
        // services.AddScoped<IStrategyRepository, StrategyRepository>();
        // services.AddScoped<IStrategyExecutionService, StrategyExecutionService>();
        // services.AddScoped<IBacktestService, BacktestService>();
        // services.AddScoped<ICSharpExecutionEngine, CSharpExecutionEngine>();
        // services.AddScoped<IPythonExecutionEngine, PythonExecutionEngine>();
        // services.AddScoped<IJuliaExecutionEngine, JuliaExecutionEngine>();

        return services;
    }
}
