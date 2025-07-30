using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Shared.Enums;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Evento para notificar dados disponíveis no MongoDB
/// </summary>
public record DataAvailableEvent : DomainEvent
{
    public override string EventType => "DataAvailableEvent";
    
    public Guid RequestId { get; init; }
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string CollectionName { get; init; } = null!;
    public string DatabaseName { get; init; } = null!;
    public string DataType { get; init; } = null!; // "MarketData", "OnChain", "News", etc.
    public Symbol? Symbol { get; init; }
    public TimeFrame? TimeFrame { get; init; }
    public DateTime DataTimestamp { get; init; }
    public int RecordCount { get; init; }
    public long DataSizeBytes { get; init; }
    public Dictionary<string, object> Schema { get; init; } = new();
    public string QualityScore { get; init; } = "Unknown"; // "High", "Medium", "Low", "Unknown"
}
