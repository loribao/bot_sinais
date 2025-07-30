using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;

namespace BotSinais.Domain.Shared.Events.Strategies;

/// <summary>
/// Evento disparado quando uma execução de estratégia é iniciada
/// </summary>
public record StrategyExecutionStartedEvent : DomainEvent
{
    public override string EventType => "StrategyExecutionStarted";
    
    public Guid ExecutionId { get; init; }
    public Guid StrategyId { get; init; }
    public string StrategyName { get; init; } = null!;
    public ExecutionLanguage Language { get; init; }
    public DateTime StartTime { get; init; }
    public Dictionary<string, object> Parameters { get; init; } = new();
}
