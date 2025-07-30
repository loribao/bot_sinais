using BotSinais.Domain.Shared.Events;

namespace BotSinais.Domain.Shared.Events.Strategies;

/// <summary>
/// Evento disparado quando um backtest é concluído
/// </summary>
public record BacktestCompletedEvent : DomainEvent
{
    public override string EventType => "BacktestCompleted";
    
    public Guid BacktestId { get; init; }
    public Guid StrategyId { get; init; }
    public string StrategyName { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public decimal InitialCapital { get; init; }
    public decimal FinalCapital { get; init; }
    public decimal TotalReturn { get; init; }
    public decimal MaxDrawdown { get; init; }
    public decimal SharpeRatio { get; init; }
    public int TotalTrades { get; init; }
    public decimal WinRate { get; init; }
}
