using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Signals.Entities;

/// <summary>
/// Entidade que representa trades individuais de uma posição
/// </summary>
public class PositionTrade : BaseEntity
{
    [Required]
    public Guid PositionId { get; set; }
    
    [Required]
    public Guid? SignalId { get; set; }  // Sinal que originou o trade
    
    [Required]
    public TradeDirection Direction { get; set; }
    
    [Required]
    public Volume Quantity { get; set; }
    
    [Required]
    public Price Price { get; set; } = null!;
    
    [Required]
    public DateTime TradeTime { get; set; }
    
    public decimal Commission { get; set; }
    public decimal Slippage { get; set; }
    
    [MaxLength(100)]
    public string? OrderId { get; set; }
    
    // Relacionamentos
    public virtual Position Position { get; set; } = null!;
}



