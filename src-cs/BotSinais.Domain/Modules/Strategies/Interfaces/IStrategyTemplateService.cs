using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.Strategies.Entities;

namespace BotSinais.Domain.Modules.Strategies.Interfaces;

/// <summary>
/// Interface para serviço de templates de estratégias
/// </summary>
public interface IStrategyTemplateService
{
    Task<StrategyTemplate?> GetTemplateAsync(Guid templateId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StrategyTemplate>> GetTemplatesByLanguageAsync(ExecutionLanguage language, CancellationToken cancellationToken = default);
    Task<IEnumerable<StrategyTemplate>> GetPublicTemplatesAsync(CancellationToken cancellationToken = default);
    
    Task<Strategy> CreateStrategyFromTemplateAsync(
        Guid templateId,
        string strategyName,
        Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default);
    
    Task<string> GenerateCodeFromTemplateAsync(
        Guid templateId,
        Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default);
}



