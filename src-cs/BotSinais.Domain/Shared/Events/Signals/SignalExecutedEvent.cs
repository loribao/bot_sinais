using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Shared.Events.Signals;

/// <summary>
/// Evento disparado quando um sinal é executado
/// </summary>
public record SignalExecutedEvent : DomainEvent
{
    public override string EventType => "SignalExecuted";
    
    public Guid SignalId { get; init; }
    public Guid ExecutionId { get; init; }
    public Guid InstrumentId { get; init; }
    public Symbol Symbol { get; init; } = null!;
    public TradeDirection Direction { get; init; }
    public Price ExecutionPrice { get; init; } = null!;
    public Volume ExecutedQuantity { get; init; }
    public DateTime ExecutionTime { get; init; }
    public decimal? Slippage { get; init; }
    public string? OrderId { get; init; }
}
