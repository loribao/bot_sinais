using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Evento disparado quando um RPA é removido do sistema
/// </summary>
public record RpaUnregisteredEvent : DomainEvent
{
    public override string EventType => "RpaUnregisteredEvent";
    
    public Guid RpaInstanceId { get; init; }
    public string Name { get; init; } = null!;
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string Reason { get; init; } = null!;
    public DateTime UnregisteredAt { get; init; } = DateTime.UtcNow;
}
