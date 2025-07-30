using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Shared.Events.Signals;

/// <summary>
/// Evento disparado quando o status de um sinal é alterado
/// </summary>
public record SignalStatusChangedEvent : DomainEvent
{
    public override string EventType => "SignalStatusChanged";
    
    public Guid SignalId { get; init; }
    public Guid InstrumentId { get; init; }
    public Symbol Symbol { get; init; } = null!;
    public SignalStatus PreviousStatus { get; init; }
    public SignalStatus NewStatus { get; init; }
    public string? Reason { get; init; }
    public DateTime ChangeTime { get; init; }
}
