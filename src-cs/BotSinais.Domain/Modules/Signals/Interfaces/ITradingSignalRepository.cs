using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.Signals.Entities;

namespace BotSinais.Domain.Modules.Signals.Interfaces;

/// <summary>
/// Interface para repositório de sinais de trading
/// </summary>
public interface ITradingSignalRepository
{
    Task<TradingSignal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TradingSignal>> GetByStatusAsync(SignalStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<TradingSignal>> GetByInstrumentAsync(Guid instrumentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TradingSignal>> GetByStrategyAsync(Guid strategyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TradingSignal>> GetActiveSignalsAsync(CancellationToken cancellationToken = default);
    Task<TradingSignal> CreateAsync(TradingSignal signal, CancellationToken cancellationToken = default);
    Task<TradingSignal> UpdateAsync(TradingSignal signal, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}



