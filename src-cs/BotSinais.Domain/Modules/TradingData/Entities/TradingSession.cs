using BotSinais.Domain.Shared.Entities;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.TradingData.Entities;

/// <summary>
/// Entidade que representa sessões de trading
/// </summary>
public class TradingSession : BaseEntity
{
    [Required]
    public Guid InstrumentId { get; set; }
    
    [Required]
    public DateTime StartTime { get; set; }
    
    [Required]
    public DateTime EndTime { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string SessionType { get; set; } = null!; // Regular, PreMarket, AfterHours
    
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = null!; // Open, Closed, Halted
    
    public Price? OpeningPrice { get; set; }
    public Price? ClosingPrice { get; set; }
    public Price? HighPrice { get; set; }
    public Price? LowPrice { get; set; }
    public Volume? TotalVolume { get; set; }
    
    // Relacionamentos
    public virtual Instrument Instrument { get; set; } = null!;
}



