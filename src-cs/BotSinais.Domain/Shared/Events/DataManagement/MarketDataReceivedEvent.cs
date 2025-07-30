using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Shared.Events.DataManagement;

/// <summary>
/// Evento disparado quando novos dados de mercado são recebidos
/// </summary>
public record MarketDataReceivedEvent : DomainEvent
{
    public override string EventType => "MarketDataReceived";
    
    public Guid InstrumentId { get; init; }
    public Symbol Symbol { get; init; } = null!;
    public TimeFrame TimeFrame { get; init; }
    public DateTime Timestamp { get; init; }
    public Price Open { get; init; } = null!;
    public Price High { get; init; } = null!;
    public Price Low { get; init; } = null!;
    public Price Close { get; init; } = null!;
    public Volume Volume { get; init; }
}
