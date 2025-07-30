using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.TradingData.Entities;

namespace BotSinais.Domain.Modules.TradingData.Interfaces;

/// <summary>
/// Interface para serviço de validação de dados
/// </summary>
public interface IDataValidationService
{
    Task<bool> ValidateMarketDataAsync(MarketData marketData, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetValidationErrorsAsync(MarketData marketData, CancellationToken cancellationToken = default);
    Task<bool> IsDataCompleteAsync(
        Guid instrumentId, 
        TimeFrame timeFrame, 
        DateTime from, 
        DateTime to, 
        CancellationToken cancellationToken = default);
}



