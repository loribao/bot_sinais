using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Shared.Events.Signals;

/// <summary>
/// Evento disparado quando uma posição é fechada
/// </summary>
public record PositionClosedEvent : DomainEvent
{
    public override string EventType => "PositionClosed";
    
    public Guid PositionId { get; init; }
    public Guid PortfolioId { get; init; }
    public Guid InstrumentId { get; init; }
    public Symbol Symbol { get; init; } = null!;
    public TradeDirection Direction { get; init; }
    public Volume Quantity { get; init; }
    public Price EntryPrice { get; init; } = null!;
    public Price ExitPrice { get; init; } = null!;
    public DateTime OpenTime { get; init; }
    public DateTime CloseTime { get; init; }
    public decimal RealizedPnL { get; init; }
}
