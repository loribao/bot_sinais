using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Evento disparado quando um health check é realizado em um RPA
/// </summary>
public record RpaHealthCheckEvent : DomainEvent
{
    public override string EventType => "RpaHealthCheckEvent";
    
    public Guid RpaInstanceId { get; init; }
    public string Name { get; init; } = null!;
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string HealthStatus { get; init; } = null!; // "Healthy", "Unhealthy", "Timeout", "Error"
    public TimeSpan ResponseTime { get; init; }
    public string? ErrorMessage { get; init; }
    public decimal? CpuUsagePercent { get; init; }
    public decimal? MemoryUsageMb { get; init; }
    public Dictionary<string, object> Metrics { get; init; } = new();
    public DateTime CheckTimestamp { get; init; } = DateTime.UtcNow;
}
