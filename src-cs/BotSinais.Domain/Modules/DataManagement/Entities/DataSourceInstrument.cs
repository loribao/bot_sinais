using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.DataManagement.Entities;

/// <summary>
/// Entidade de relacionamento entre DataSource e Instrument
/// </summary>
public class DataSourceInstrument : BaseEntity
{
    [Required]
    public Guid DataSourceId { get; set; }
    
    [Required]
    public Guid InstrumentId { get; set; }
    
    [MaxLength(100)]
    public string? ExternalSymbol { get; set; } // Símbolo usado pela fonte de dados
    
    public new bool IsActive { get; set; } = true;
    
    // Relacionamentos
    public virtual DataSource DataSource { get; set; } = null!;
    public virtual Instrument Instrument { get; set; } = null!;
}



