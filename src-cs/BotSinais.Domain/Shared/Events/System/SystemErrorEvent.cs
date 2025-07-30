using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Shared.Events.System;

/// <summary>
/// Evento disparado quando há uma falha no sistema
/// </summary>
public record SystemErrorEvent : DomainEvent
{
    public override string EventType => "SystemError";
    
    public string Component { get; init; } = null!;
    public string ErrorType { get; init; } = null!;
    public string Message { get; init; } = null!;
    public string? StackTrace { get; init; }
    public Dictionary<string, object> Context { get; init; } = new();
}
