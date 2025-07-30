using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Evento disparado quando um RPA é registrado no sistema
/// </summary>
public record RpaRegisteredEvent : DomainEvent
{
    public override string EventType => "RpaRegisteredEvent";
    
    public Guid RpaInstanceId { get; init; }
    public string Name { get; init; } = null!;
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string Version { get; init; } = null!;
    public string ConnectionString { get; init; } = null!;
    public Dictionary<string, object> Configuration { get; init; } = new();
}
