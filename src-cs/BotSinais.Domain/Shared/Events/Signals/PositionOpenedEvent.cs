using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Shared.Events.Signals;

/// <summary>
/// Evento disparado quando uma posição é aberta
/// </summary>
public record PositionOpenedEvent : DomainEvent
{
    public override string EventType => "PositionOpened";
    
    public Guid PositionId { get; init; }
    public Guid PortfolioId { get; init; }
    public Guid InstrumentId { get; init; }
    public Symbol Symbol { get; init; } = null!;
    public TradeDirection Direction { get; init; }
    public Volume Quantity { get; init; }
    public Price EntryPrice { get; init; } = null!;
    public DateTime OpenTime { get; init; }
}
