using BotSinais.Domain.Shared.Entities;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Signals.Entities;

/// <summary>
/// Entidade de relacionamento entre Portfolio e TradingSignal
/// </summary>
public class PortfolioSignal : BaseEntity
{
    [Required]
    public Guid PortfolioId { get; set; }
    
    [Required]
    public Guid SignalId { get; set; }
    
    [Required]
    public DateTime AssignedAt { get; set; }
    
    public new bool IsActive { get; set; } = true;
    
    // Configurações específicas do sinal para este portfolio
    public decimal? CustomQuantity { get; set; }
    public decimal? CustomRiskLevel { get; set; }
    
    // Relacionamentos
    public virtual Portfolio Portfolio { get; set; } = null!;
    public virtual TradingSignal Signal { get; set; } = null!;
}



