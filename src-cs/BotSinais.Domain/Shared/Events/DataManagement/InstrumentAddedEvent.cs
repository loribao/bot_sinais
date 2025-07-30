using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Shared.Events.DataManagement;

/// <summary>
/// Evento disparado quando um novo instrumento é adicionado
/// </summary>
public record InstrumentAddedEvent : DomainEvent
{
    public override string EventType => "InstrumentAdded";
    
    public Guid InstrumentId { get; init; }
    public Symbol Symbol { get; init; } = null!;
    public InstrumentType Type { get; init; }
    public string Name { get; init; } = null!;
    public string BaseCurrency { get; init; } = null!;
}
