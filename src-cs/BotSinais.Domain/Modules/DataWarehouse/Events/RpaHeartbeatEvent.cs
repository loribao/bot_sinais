using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Evento disparado quando um RPA envia heartbeat
/// </summary>
public record RpaHeartbeatEvent : DomainEvent
{
    public override string EventType => "RpaHeartbeatEvent";
    
    public Guid RpaInstanceId { get; init; }
    public string Name { get; init; } = null!;
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string Status { get; init; } = null!;
    public int ActiveRequests { get; init; }
    public decimal? CpuUsagePercent { get; init; }
    public decimal? MemoryUsageMb { get; init; }
    public Dictionary<string, object> Metrics { get; init; } = new();
    public DateTime HeartbeatTimestamp { get; init; } = DateTime.UtcNow;
}
