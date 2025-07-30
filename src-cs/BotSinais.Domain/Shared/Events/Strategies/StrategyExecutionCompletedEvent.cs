using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Shared.Events.Strategies;

/// <summary>
/// Evento disparado quando uma execução de estratégia é finalizada
/// </summary>
public record StrategyExecutionCompletedEvent : DomainEvent
{
    public override string EventType => "StrategyExecutionCompleted";
    
    public Guid ExecutionId { get; init; }
    public Guid StrategyId { get; init; }
    public string StrategyName { get; init; } = null!;
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Status { get; init; } = null!;
    public int SignalsGenerated { get; init; }
    public decimal? PnL { get; init; }
    public string? ErrorMessage { get; init; }
}
