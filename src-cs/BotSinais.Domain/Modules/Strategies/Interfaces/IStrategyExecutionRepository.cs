using BotSinais.Domain.Modules.Strategies.Entities;

namespace BotSinais.Domain.Modules.Strategies.Interfaces;

/// <summary>
/// Interface para repositório de execuções de estratégias
/// </summary>
public interface IStrategyExecutionRepository
{
    Task<StrategyExecution?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StrategyExecution>> GetByStrategyAsync(Guid strategyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StrategyExecution>> GetRunningExecutionsAsync(CancellationToken cancellationToken = default);
    Task<StrategyExecution> CreateAsync(StrategyExecution execution, CancellationToken cancellationToken = default);
    Task<StrategyExecution> UpdateAsync(StrategyExecution execution, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


