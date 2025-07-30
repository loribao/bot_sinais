using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Shared.Enums;

namespace BotSinais.Domain.Modules.DataWarehouse.Events;

/// <summary>
/// Comando para RPA iniciar coleta de dados
/// </summary>
public record StartDataCollectionCommand : DomainEvent
{
    public override string EventType => "StartDataCollectionCommand";
    
    public Guid RequestId { get; init; }
    public string RpaType { get; init; } = null!; // "MarketData", "OnChain", "News", "Social"
    public string DataSource { get; init; } = null!; // "Binance", "Coinbase", "Etherscan", "Twitter"
    public Symbol? Symbol { get; init; }
    public TimeFrame? TimeFrame { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public Dictionary<string, object> Parameters { get; init; } = new();
    public int Priority { get; init; } = 5; // 1-10, onde 10 é máxima prioridade
    public TimeSpan Timeout { get; init; } = TimeSpan.FromMinutes(30);
}
