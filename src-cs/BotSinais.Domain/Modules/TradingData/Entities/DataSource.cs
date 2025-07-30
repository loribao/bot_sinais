using BotSinais.Domain.Shared.Entities;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.TradingData.Entities;

/// <summary>
/// Entidade que representa fontes de dados
/// </summary>
public class DataSource : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = null!; // REST, WebSocket, FIX, File
    
    [Required]
    [MaxLength(500)]
    public string ConnectionString { get; set; } = null!;
    
    public bool IsRealTime { get; set; }
    public new bool IsActive { get; set; } = true;
    
    public int Priority { get; set; } = 1;
    public int RetryAttempts { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);
    
    // Configurações específicas da fonte
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    // Relacionamentos
    public virtual ICollection<DataSourceInstrument> SupportedInstruments { get; set; } = new List<DataSourceInstrument>();
}



