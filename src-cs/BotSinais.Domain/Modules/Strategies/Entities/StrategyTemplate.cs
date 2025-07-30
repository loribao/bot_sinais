using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Strategies.Entities;

/// <summary>
/// Entidade que representa templates de estratégias
/// </summary>
public class StrategyTemplate : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public StrategyType Type { get; set; }
    
    [Required]
    public ExecutionLanguage Language { get; set; }
    
    [Required]
    public string TemplateCode { get; set; } = null!;  // Código template
    
    [MaxLength(50)]
    public string Category { get; set; } = "General";
    
    public bool IsPublic { get; set; } = true;
    
    // Parâmetros configuráveis do template
    public Dictionary<string, string> TemplateParameters { get; set; } = new();
    public Dictionary<string, object> DefaultValues { get; set; } = new();
    
    [MaxLength(2000)]
    public string? Documentation { get; set; }
    
    // Relacionamentos
    public virtual ICollection<Strategy> CreatedStrategies { get; set; } = new List<Strategy>();
}



