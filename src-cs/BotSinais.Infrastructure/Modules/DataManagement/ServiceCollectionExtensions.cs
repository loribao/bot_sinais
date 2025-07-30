using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BotSinais.Infrastructure.Modules.DataManagement;

/// <summary>
/// Extensões de configuração para o módulo de Gerenciamento de Dados
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona serviços de gerenciamento de dados de mercado
    /// </summary>
    public static IServiceCollection AddDataManagementModule(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: Configuração do Entity Framework
        // services.AddDbContext<BotSinaisDbContext>(options =>
        //     options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // TODO: Adicionar repositórios e serviços específicos de dados
        // services.AddScoped<IInstrumentRepository, InstrumentRepository>();
        // services.AddScoped<IMarketDataRepository, MarketDataRepository>();
        // services.AddScoped<IDataProviderService, DataProviderService>();

        return services;
    }
}
