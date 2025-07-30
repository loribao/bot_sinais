using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Strategies.Entities;

/// <summary>
/// Entidade que representa engines de execução
/// </summary>
public class ExecutionEngine : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [Required]
    public ExecutionLanguage Language { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string ExecutablePath { get; set; } = null!;
    
    [MaxLength(500)]
    public string? WorkingDirectory { get; set; }
    
    [MaxLength(1000)]
    public string? Arguments { get; set; }
    
    public new bool IsActive { get; set; } = true;
    
    [MaxLength(20)]
    public string Version { get; set; } = "1.0.0";
    
    // Configurações específicas do engine
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    // Limites de recursos
    public int MaxMemoryMB { get; set; } = 1024;
    public int MaxExecutionTimeMinutes { get; set; } = 60;
    public int MaxConcurrentExecutions { get; set; } = 5;
}



