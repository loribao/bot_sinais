namespace BotSinais.Domain.Modules.Signals.Interfaces;

/// <summary>
/// Interface para serviço de análise de performance
/// </summary>
public interface IPerformanceAnalysisService
{
    Task<decimal> CalculateROIAsync(Guid portfolioId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<decimal> CalculateSharpeRatioAsync(Guid portfolioId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<decimal> CalculateWinRateAsync(Guid portfolioId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
    
    Task<IDictionary<string, decimal>> GetPerformanceMetricsAsync(
        Guid portfolioId, 
        DateTime from, 
        DateTime to, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<decimal>> GetDailyReturnsAsync(
        Guid portfolioId, 
        DateTime from, 
        DateTime to, 
        CancellationToken cancellationToken = default);
}


