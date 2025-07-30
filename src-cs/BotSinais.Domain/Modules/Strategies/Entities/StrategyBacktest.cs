using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Strategies.Entities;

/// <summary>
/// Entidade que representa um backtest de estratégia
/// </summary>
public class StrategyBacktest : BaseEntity
{
    [Required]
    public Guid StrategyId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public DateTime ExecutionTime { get; set; }
    
    public decimal InitialCapital { get; set; }
    public decimal FinalCapital { get; set; }
    public decimal TotalReturn { get; set; }
    public decimal MaxDrawdown { get; set; }
    public decimal SharpeRatio { get; set; }
    public decimal SortinoRatio { get; set; }
    public decimal CalmarRatio { get; set; }
    
    public int TotalTrades { get; set; }
    public int WinningTrades { get; set; }
    public int LosingTrades { get; set; }
    public decimal WinRate { get; set; }
    public decimal AvgWin { get; set; }
    public decimal AvgLoss { get; set; }
    public decimal ProfitFactor { get; set; }
    
    // Parâmetros utilizados no backtest
    public Dictionary<string, object> BacktestParameters { get; set; } = new();
    
    // Configurações do backtest
    public Dictionary<string, object> BacktestConfig { get; set; } = new();
    
    [MaxLength(5000)]
    public string? Notes { get; set; }
    
    // Relacionamentos
    public virtual Strategy Strategy { get; set; } = null!;
    public virtual ICollection<BacktestTrade> Trades { get; set; } = new List<BacktestTrade>();
}



