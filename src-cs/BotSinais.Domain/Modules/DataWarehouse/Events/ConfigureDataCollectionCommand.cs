using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Comando para RPA configurar parâmetros de coleta
/// </summary>
public record ConfigureDataCollectionCommand : DomainEvent
{
    public override string EventType => "ConfigureDataCollectionCommand";
    
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public Dictionary<string, object> Configuration { get; init; } = new();
    public TimeSpan CollectionInterval { get; init; } = TimeSpan.FromMinutes(1);
    public int RetryAttempts { get; init; } = 3;
    public TimeSpan RetryDelay { get; init; } = TimeSpan.FromSeconds(30);
}
