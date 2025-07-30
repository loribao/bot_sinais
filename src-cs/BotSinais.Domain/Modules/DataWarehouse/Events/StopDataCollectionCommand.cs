using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Comando para RPA parar coleta de dados
/// </summary>
public record StopDataCollectionCommand : DomainEvent
{
    public override string EventType => "StopDataCollectionCommand";
    
    public Guid RequestId { get; init; }
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string Reason { get; init; } = "Manual stop";
}
