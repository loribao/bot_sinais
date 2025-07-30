using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Signals.Entities;

/// <summary>
/// Entidade que representa uma carteira de trading
/// </summary>
public class Portfolio : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Owner { get; set; } = null!;
    
    public decimal InitialCapital { get; set; }
    public decimal CurrentCapital { get; set; }
    public decimal MaxDrawdown { get; set; }
    
    public new bool IsActive { get; set; } = true;
    
    // Configurações de risco
    public decimal MaxRiskPerTrade { get; set; } = 0.02m; // 2% por padrão
    public decimal MaxPortfolioRisk { get; set; } = 0.20m; // 20% por padrão
    
    // Relacionamentos
    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
    public virtual ICollection<PortfolioSignal> Signals { get; set; } = new List<PortfolioSignal>();
}



