using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Evento de resposta do RPA sobre status da coleta
/// </summary>
public record DataCollectionStatusEvent : DomainEvent
{
    public override string EventType => "DataCollectionStatusEvent";
    
    public Guid RequestId { get; init; }
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string Status { get; init; } = null!; // "Started", "Running", "Completed", "Failed", "Stopped"
    public string? Message { get; init; }
    public Dictionary<string, object> Metadata { get; init; } = new();
    public DateTime LastActivity { get; init; }
    public int RecordsProcessed { get; init; }
    public string? ErrorDetails { get; init; }
}
