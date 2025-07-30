namespace BotSinais.Domain.Modules.Strategies.Interfaces;

/// <summary>
/// Interface para serviço de análise de estratégias
/// </summary>
public interface IStrategyAnalysisService
{
    Task<IDictionary<string, decimal>> AnalyzeStrategyPerformanceAsync(
        Guid strategyId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);
    
    Task<decimal> CalculateStrategyCorrelationAsync(
        Guid strategyId1,
        Guid strategyId2,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<string>> GetStrategyRecommendationsAsync(
        Guid strategyId,
        CancellationToken cancellationToken = default);
    
    Task<IDictionary<string, object>> GetStrategyStatisticsAsync(
        Guid strategyId,
        CancellationToken cancellationToken = default);
}


