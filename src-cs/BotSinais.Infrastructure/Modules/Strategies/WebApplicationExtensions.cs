using Microsoft.AspNetCore.Builder;

namespace BotSinais.Infrastructure.Modules.Strategies;

/// <summary>
/// Extensões de configuração de pipeline para o módulo de Estratégias
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configura pipeline de estratégias
    /// </summary>
    public static WebApplication UseStrategiesModule(this WebApplication app)
    {
        // Configurações específicas do módulo de estratégias podem ser adicionadas aqui
        // Por exemplo, middlewares para execução segura de estratégias
        
        return app;
    }
}
