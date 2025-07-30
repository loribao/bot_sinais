using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.DataManagement.Entities;

namespace BotSinais.Domain.Modules.DataManagement.Interfaces;

/// <summary>
/// Interface para repositório de dados de mercado
/// </summary>
public interface IMarketDataRepository
{
    Task<MarketData?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MarketData>> GetByInstrumentAsync(
        Guid instrumentId, 
        TimeFrame timeFrame, 
        DateTime from, 
        DateTime to, 
        CancellationToken cancellationToken = default);
    
    Task<MarketData?> GetLatestAsync(
        Guid instrumentId, 
        TimeFrame timeFrame, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<MarketData>> CreateBatchAsync(
        IEnumerable<MarketData> marketData, 
        CancellationToken cancellationToken = default);
    
    Task<MarketData> CreateAsync(MarketData marketData, CancellationToken cancellationToken = default);
    Task<MarketData> UpdateAsync(MarketData marketData, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}



