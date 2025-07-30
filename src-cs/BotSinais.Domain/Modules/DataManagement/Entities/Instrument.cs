using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.DataManagement.Entities;

/// <summary>
/// Entidade que representa um instrumento financeiro
/// </summary>
public class Instrument : BaseEntity
{
    [Required]
    public Symbol Symbol { get; set; } = null!;
    
    [Required]
    public InstrumentType Type { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal TickSize { get; set; }
    public long MinVolume { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string BaseCurrency { get; set; } = null!;
    
    [MaxLength(10)]
    public string? QuoteCurrency { get; set; }
    
    public new bool IsActive { get; set; } = true;
    public DateTime? ListedDate { get; set; }
    public DateTime? DelistedDate { get; set; }
    
    // Configurações específicas do instrumento
    public Dictionary<string, object> Metadata { get; set; } = new();
    
    // Relacionamentos
    public virtual ICollection<MarketData> MarketData { get; set; } = new List<MarketData>();
    public virtual ICollection<TradingSession> TradingSessions { get; set; } = new List<TradingSession>();
}



