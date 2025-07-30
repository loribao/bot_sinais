using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;

namespace BotSinais.Domain.Shared.Events.System;

/// <summary>
/// Evento disparado para solicitação de análise de dados
/// </summary>
public record DataAnalysisRequestEvent : DomainEvent
{
    public override string EventType => "DataAnalysisRequest";
    
    public Guid InstrumentId { get; init; }
    public TimeFrame TimeFrame { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
    public string AnalysisType { get; init; } = null!;
    public Dictionary<string, object> Parameters { get; init; } = new();
}
