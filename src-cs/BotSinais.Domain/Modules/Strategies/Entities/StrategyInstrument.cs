using BotSinais.Domain.Shared.Entities;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Strategies.Entities;

/// <summary>
/// Entidade de relacionamento entre Strategy e Instrument
/// </summary>
public class StrategyInstrument : BaseEntity
{
    [Required]
    public Guid StrategyId { get; set; }
    
    [Required]
    public Guid InstrumentId { get; set; }
    
    public new bool IsActive { get; set; } = true;
    
    // Parâmetros específicos para este instrumento
    public Dictionary<string, object> InstrumentParameters { get; set; } = new();
    
    // Relacionamentos
    public virtual Strategy Strategy { get; set; } = null!;
}



