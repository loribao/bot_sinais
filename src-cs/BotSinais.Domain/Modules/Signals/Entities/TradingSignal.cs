using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Signals.Entities;

/// <summary>
/// Entidade que representa um sinal de trading
/// </summary>
public class TradingSignal : BaseEntity, IVersionedEntity
{
    [Required]
    public Guid InstrumentId { get; set; }
    
    [Required]
    public Guid StrategyId { get; set; }
    
    [Required]
    public TradeDirection Direction { get; set; }
    
    [Required]
    public SignalStatus Status { get; set; } = SignalStatus.Generated;
    
    [Required]
    public Price EntryPrice { get; set; } = null!;
    
    public Price? StopLoss { get; set; }
    public Price? TakeProfit { get; set; }
    
    [Required]
    public Volume Quantity { get; set; }
    
    [Required]
    public DateTime SignalTime { get; set; }
    
    public DateTime? ExpiryTime { get; set; }
    public DateTime? ExecutionTime { get; set; }
    
    [Range(0, 100)]
    public decimal Confidence { get; set; }  // 0-100%
    
    [Range(1, 5)]
    public int Risk { get; set; } = 3;  // 1-5 scale
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [MaxLength(500)]
    public string? Rationale { get; set; }  // Justificativa do sinal
    
    public int Version { get; set; } = 1;
    
    // Dados técnicos que geraram o sinal
    public Dictionary<string, object> TechnicalData { get; set; } = new();
    
    // Relacionamentos
    public virtual SignalExecution? Execution { get; set; }
    public virtual ICollection<SignalUpdate> Updates { get; set; } = new List<SignalUpdate>();
}



