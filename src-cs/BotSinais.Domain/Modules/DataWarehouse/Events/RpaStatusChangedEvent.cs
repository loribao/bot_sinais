using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Evento disparado quando um RPA muda de status
/// </summary>
public record RpaStatusChangedEvent : DomainEvent
{
    public override string EventType => "RpaStatusChangedEvent";
    
    public Guid RpaInstanceId { get; init; }
    public string Name { get; init; } = null!;
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string PreviousStatus { get; init; } = null!;
    public string NewStatus { get; init; } = null!;
    public string? Reason { get; init; }
    public DateTime ChangedAt { get; init; } = DateTime.UtcNow;
}
