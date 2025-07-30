using Microsoft.AspNetCore.Builder;

namespace BotSinais.Infrastructure.Modules.Signals;

/// <summary>
/// Extensões de configuração de pipeline para o módulo de Sinais
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configura pipeline de sinais
    /// </summary>
    public static WebApplication UseSignalsModule(this WebApplication app)
    {
        // Configurações específicas do módulo de sinais podem ser adicionadas aqui
        // Por exemplo, middlewares específicos para processamento de sinais
        
        return app;
    }
}
