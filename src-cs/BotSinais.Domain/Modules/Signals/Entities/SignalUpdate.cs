using BotSinais.Domain.Shared.Entities;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Signals.Entities;

/// <summary>
/// Entidade que representa atualizações de um sinal
/// </summary>
public class SignalUpdate : BaseEntity
{
    [Required]
    public Guid SignalId { get; set; }
    
    [Required]
    public SignalStatus PreviousStatus { get; set; }
    
    [Required]
    public SignalStatus NewStatus { get; set; }
    
    [Required]
    public DateTime UpdateTime { get; set; }
    
    [MaxLength(500)]
    public string? Reason { get; set; }
    
    [MaxLength(50)]
    public new string? UpdatedBy { get; set; }
    
    // Relacionamentos
    public virtual TradingSignal Signal { get; set; } = null!;
}



