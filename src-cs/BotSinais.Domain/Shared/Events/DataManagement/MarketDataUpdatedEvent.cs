using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Shared.Events.DataManagement;

/// <summary>
/// Evento disparado quando dados de mercado são atualizados
/// </summary>
public record MarketDataUpdatedEvent : DomainEvent
{
    public override string EventType => "MarketDataUpdated";
    
    public Guid MarketDataId { get; init; }
    public Guid InstrumentId { get; init; }
    public Symbol Symbol { get; init; } = null!;
    public TimeFrame TimeFrame { get; init; }
    public DateTime Timestamp { get; init; }
    public Price PreviousClose { get; init; } = null!;
    public Price NewClose { get; init; } = null!;
}
