using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.Strategies.Entities;

namespace BotSinais.Domain.Modules.Strategies.Interfaces;

/// <summary>
/// Interface para repositório de estratégias
/// </summary>
public interface IStrategyRepository
{
    Task<Strategy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Strategy?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Strategy>> GetByTypeAsync(StrategyType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Strategy>> GetByLanguageAsync(ExecutionLanguage language, CancellationToken cancellationToken = default);
    Task<IEnumerable<Strategy>> GetActiveStrategiesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Strategy>> GetPublicStrategiesAsync(CancellationToken cancellationToken = default);
    Task<Strategy> CreateAsync(Strategy strategy, CancellationToken cancellationToken = default);
    Task<Strategy> UpdateAsync(Strategy strategy, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}



