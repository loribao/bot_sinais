using Microsoft.AspNetCore.Builder;

namespace BotSinais.Infrastructure.Modules.DataManagement;

/// <summary>
/// Extensões de configuração de pipeline para o módulo de Gerenciamento de Dados
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configura pipeline de gerenciamento de dados
    /// </summary>
    public static WebApplication UseDataManagementModule(this WebApplication app)
    {
        // Configurações específicas do módulo de dados podem ser adicionadas aqui
        // Por exemplo, middlewares para validação de dados de mercado
        
        return app;
    }
}
