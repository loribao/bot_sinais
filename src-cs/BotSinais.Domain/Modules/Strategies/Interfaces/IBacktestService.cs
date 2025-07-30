using BotSinais.Domain.Modules.Strategies.Entities;

namespace BotSinais.Domain.Modules.Strategies.Interfaces;

/// <summary>
/// Interface para serviço de backtest
/// </summary>
public interface IBacktestService
{
    Task<StrategyBacktest> RunBacktestAsync(
        Guid strategyId,
        DateTime startDate,
        DateTime endDate,
        decimal initialCapital,
        Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default);
    
    Task<bool> ValidateBacktestParametersAsync(
        Guid strategyId,
        DateTime startDate,
        DateTime endDate,
        Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default);
    
    Task<StrategyBacktest> GetBacktestResultsAsync(Guid backtestId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<BacktestTrade>> GetBacktestTradesAsync(
        Guid backtestId, 
        CancellationToken cancellationToken = default);
}


