using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Strategies.Entities;

/// <summary>
/// Entidade que representa uma estratégia de trading
/// </summary>
public class Strategy : BaseEntity, IVersionedEntity
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
    [MaxLength(50)]
    public string Author { get; set; } = null!;
    
    public int Version { get; set; } = 1;
    
    [Required]
    public string SourceCode { get; set; } = null!;  // Código fonte da estratégia
    
    [MaxLength(500)]
    public string? EntryPoint { get; set; }  // Função/método de entrada
    
    public new bool IsActive { get; set; } = true;
    public bool IsPublic { get; set; } = false;
    
    // Configurações da estratégia
    public Dictionary<string, object> Parameters { get; set; } = new();
    public Dictionary<string, string> ParameterTypes { get; set; } = new();  // Tipos dos parâmetros
    public Dictionary<string, object> DefaultValues { get; set; } = new();   // Valores padrão
    
    // Metadados de performance
    public decimal? BacktestReturn { get; set; }
    public decimal? MaxDrawdown { get; set; }
    public decimal? SharpeRatio { get; set; }
    public int? TotalTrades { get; set; }
    public decimal? WinRate { get; set; }
    
    // Relacionamentos
    public virtual ICollection<StrategyExecution> Executions { get; set; } = new List<StrategyExecution>();
    public virtual ICollection<StrategyBacktest> Backtests { get; set; } = new List<StrategyBacktest>();
    public virtual ICollection<StrategyInstrument> SupportedInstruments { get; set; } = new List<StrategyInstrument>();
}



