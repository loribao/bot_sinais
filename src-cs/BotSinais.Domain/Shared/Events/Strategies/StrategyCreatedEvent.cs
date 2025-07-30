using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;

namespace BotSinais.Domain.Shared.Events.Strategies;

/// <summary>
/// Evento disparado quando uma estratégia é criada
/// </summary>
public record StrategyCreatedEvent : DomainEvent
{
    public override string EventType => "StrategyCreated";
    
    public Guid StrategyId { get; init; }
    public string Name { get; init; } = null!;
    public StrategyType Type { get; init; }
    public ExecutionLanguage Language { get; init; }
    public string Author { get; init; } = null!;
    public int Version { get; init; }
}
