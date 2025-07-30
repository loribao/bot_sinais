using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Shared.Events.Signals;

/// <summary>
/// Evento disparado quando um novo sinal é gerado
/// </summary>
public record SignalGeneratedEvent : DomainEvent
{
    public override string EventType => "SignalGenerated";
    
    public Guid SignalId { get; init; }
    public Guid InstrumentId { get; init; }
    public Guid StrategyId { get; init; }
    public Symbol Symbol { get; init; } = null!;
    public TradeDirection Direction { get; init; }
    public Price EntryPrice { get; init; } = null!;
    public Price? StopLoss { get; init; }
    public Price? TakeProfit { get; init; }
    public Volume Quantity { get; init; }
    public decimal Confidence { get; init; }
    public DateTime SignalTime { get; init; }
    public string? Rationale { get; init; }
}
