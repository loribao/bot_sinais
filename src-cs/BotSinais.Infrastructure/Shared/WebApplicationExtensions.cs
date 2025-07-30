using BotSinais.Infrastructure.Modules.Auth;
using BotSinais.Infrastructure.Modules.DataManagement;
using BotSinais.Infrastructure.Modules.Signals;
using BotSinais.Infrastructure.Modules.Strategies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace BotSinais.Infrastructure.Shared;

/// <summary>
/// Extensões centralizadas de pipeline que unificam todos os módulos do Bot Sinais
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configura o pipeline completo do Bot Sinais com todos os módulos
    /// Este é o ponto de entrada único para configuração do pipeline
    /// </summary>
    public static WebApplication ApiConfigureBotSinaisPipeline(this WebApplication app)
    {
        // Tratamento de exceções
        app.UseExceptionHandler();

        // === MÓDULOS DO BOT SINAIS ===
        
        // Módulo de Autenticação (deve vir antes dos demais)
        app.UseAuthModule();
        
        // Módulo de Gerenciamento de Dados
        app.UseDataManagementModule();
        
        // Módulo de Sinais
        app.UseSignalsModule();
        
        // Módulo de Estratégias
        app.UseStrategiesModule();

        // OpenAPI em desenvolvimento
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // Mapeamento de controllers (todos os módulos)
        app.MapControllers();

        return app;
    }

    /// <summary>
    /// Configura pipeline específico para API
    /// </summary>
    public static WebApplication ConfigureBotSinaisApiPipeline(this WebApplication app)
    {
        // Pipeline básico com todos os módulos
        app.ApiConfigureBotSinaisPipeline();

        // Configurações específicas da API podem ser adicionadas aqui
        // Por exemplo: rate limiting, API documentation, etc.
        
        return app;
    }

    /// <summary>
    /// Configura pipeline específico para Web (Blazor)
    /// </summary>
    public static WebApplication BlazorConfigureBotSinaisWebPipeline(this WebApplication app)
    {
        // Configurações específicas do Blazor
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Mapear Blazor
        app.MapRazorPages();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        return app;
    }
}
