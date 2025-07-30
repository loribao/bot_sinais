using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Shared.Events.DataManagement;

/// <summary>
/// Evento disparado quando dados históricos são carregados
/// </summary>
public record HistoricalDataLoadedEvent : DomainEvent
{
    public override string EventType => "HistoricalDataLoaded";
    
    public Guid InstrumentId { get; init; }
    public Symbol Symbol { get; init; } = null!;
    public TimeFrame TimeFrame { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
    public int RecordsCount { get; init; }
}
