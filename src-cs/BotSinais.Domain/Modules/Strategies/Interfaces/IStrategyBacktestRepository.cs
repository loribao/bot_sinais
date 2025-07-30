using BotSinais.Domain.Modules.Strategies.Entities;

namespace BotSinais.Domain.Modules.Strategies.Interfaces;

/// <summary>
/// Interface para repositório de backtests
/// </summary>
public interface IStrategyBacktestRepository
{
    Task<StrategyBacktest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StrategyBacktest>> GetByStrategyAsync(Guid strategyId, CancellationToken cancellationToken = default);
    Task<StrategyBacktest> CreateAsync(StrategyBacktest backtest, CancellationToken cancellationToken = default);
    Task<StrategyBacktest> UpdateAsync(StrategyBacktest backtest, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


