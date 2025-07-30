using BotSinais.Domain.Modules.Signals.Entities;

namespace BotSinais.Domain.Modules.Signals.Interfaces;

/// <summary>
/// Interface para serviço de geração de sinais
/// </summary>
public interface ISignalGenerationService
{
    Task<TradingSignal> GenerateSignalAsync(
        Guid instrumentId, 
        Guid strategyId, 
        Dictionary<string, object> parameters, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<TradingSignal>> GenerateSignalsAsync(
        IEnumerable<Guid> instrumentIds, 
        Guid strategyId, 
        CancellationToken cancellationToken = default);
    
    Task<bool> ValidateSignalAsync(TradingSignal signal, CancellationToken cancellationToken = default);
}


