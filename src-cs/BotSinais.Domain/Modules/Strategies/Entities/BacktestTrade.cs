using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Strategies.Entities;

/// <summary>
/// Entidade que representa um trade de backtest
/// </summary>
public class BacktestTrade : BaseEntity
{
    [Required]
    public Guid BacktestId { get; set; }
    
    [Required]
    public Guid InstrumentId { get; set; }
    
    [Required]
    public TradeDirection Direction { get; set; }
    
    [Required]
    public DateTime EntryTime { get; set; }
    
    [Required]
    public Price EntryPrice { get; set; } = null!;
    
    [Required]
    public DateTime ExitTime { get; set; }
    
    [Required]
    public Price ExitPrice { get; set; } = null!;
    
    [Required]
    public Volume Quantity { get; set; }
    
    public decimal PnL { get; set; }
    public decimal Commission { get; set; }
    public decimal PnLPercent { get; set; }
    
    [MaxLength(500)]
    public string? ExitReason { get; set; }  // StopLoss, TakeProfit, Signal, TimeLimit
    
    // Relacionamentos
    public virtual StrategyBacktest Backtest { get; set; } = null!;
}



