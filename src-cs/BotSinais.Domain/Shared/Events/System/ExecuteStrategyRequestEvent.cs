using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Shared.Events.System;

/// <summary>
/// Evento disparado para solicitação de execução de estratégia
/// </summary>
public record ExecuteStrategyRequestEvent : DomainEvent
{
    public override string EventType => "ExecuteStrategyRequest";
    
    public Guid StrategyId { get; init; }
    public Dictionary<string, object> Parameters { get; init; } = new();
    public string RequestedBy { get; init; } = null!;
}
