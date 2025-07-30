using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.DataManagement.Entities;

namespace BotSinais.Domain.Modules.DataManagement.Interfaces;

/// <summary>
/// Interface para serviço de coleta de dados
/// </summary>
public interface IDataCollectionService
{
    Task StartCollectionAsync(Guid instrumentId, TimeFrame timeFrame, CancellationToken cancellationToken = default);
    Task StopCollectionAsync(Guid instrumentId, TimeFrame timeFrame, CancellationToken cancellationToken = default);
    Task<bool> IsCollectingAsync(Guid instrumentId, TimeFrame timeFrame, CancellationToken cancellationToken = default);
    Task<IEnumerable<MarketData>> GetHistoricalDataAsync(
        Symbol symbol, 
        TimeFrame timeFrame, 
        DateTime from, 
        DateTime to, 
        CancellationToken cancellationToken = default);
}



