using BotSinais.Domain.Shared.Entities;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Strategies.Entities;

/// <summary>
/// Entidade que representa uma execução de estratégia
/// </summary>
public class StrategyExecution : BaseEntity
{
    [Required]
    public Guid StrategyId { get; set; }
    
    [Required]
    public DateTime StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Running"; // Running, Completed, Failed, Stopped
    
    [MaxLength(50)]
    public string? ExecutionEngine { get; set; }  // C#, Python, Julia
    
    [MaxLength(100)]
    public string? ProcessId { get; set; }  // ID do processo de execução
    
    // Parâmetros utilizados nesta execução
    public Dictionary<string, object> ExecutionParameters { get; set; } = new();
    
    // Resultados da execução
    public int SignalsGenerated { get; set; }
    public int SignalsExecuted { get; set; }
    public decimal? PnL { get; set; }
    
    [MaxLength(2000)]
    public string? ErrorMessage { get; set; }
    
    [MaxLength(5000)]
    public string? Log { get; set; }
    
    // Relacionamentos
    public virtual Strategy Strategy { get; set; } = null!;
}



