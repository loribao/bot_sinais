using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Evento de erro no RPA
/// </summary>
public record RpaErrorEvent : DomainEvent
{
    public override string EventType => "RpaErrorEvent";
    
    public Guid RequestId { get; init; }
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string ErrorType { get; init; } = null!; // "Connection", "Authentication", "RateLimit", "Parsing", "Unknown"
    public string ErrorMessage { get; init; } = null!;
    public string? StackTrace { get; init; }
    public Dictionary<string, object> Context { get; init; } = new();
    public bool IsRetryable { get; init; } = true;
    public int AttemptNumber { get; init; } = 1;
    public DateTime NextRetryAt { get; init; }
}
