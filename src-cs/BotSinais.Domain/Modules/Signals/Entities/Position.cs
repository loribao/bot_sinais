using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Signals.Entities;

/// <summary>
/// Entidade que representa uma posição em um instrumento
/// </summary>
public class Position : BaseEntity
{
    [Required]
    public Guid PortfolioId { get; set; }
    
    [Required]
    public Guid InstrumentId { get; set; }
    
    [Required]
    public TradeDirection Direction { get; set; }
    
    [Required]
    public Volume Quantity { get; set; }
    
    [Required]
    public Price EntryPrice { get; set; } = null!;
    
    public Price? CurrentPrice { get; set; }
    public Price? ExitPrice { get; set; }
    
    [Required]
    public DateTime OpenTime { get; set; }
    
    public DateTime? CloseTime { get; set; }
    
    public decimal UnrealizedPnL { get; set; }
    public decimal RealizedPnL { get; set; }
    
    [MaxLength(50)]
    public string Status { get; set; } = "Open"; // Open, Closed, PartiallyFilled
    
    // Relacionamentos
    public virtual Portfolio Portfolio { get; set; } = null!;
    public virtual ICollection<PositionTrade> Trades { get; set; } = new List<PositionTrade>();
}



