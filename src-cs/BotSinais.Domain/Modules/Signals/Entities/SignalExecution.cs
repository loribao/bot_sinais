using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.Signals.Entities;

/// <summary>
/// Entidade que representa a execução de um sinal
/// </summary>
public class SignalExecution : BaseEntity
{
    [Required]
    public Guid SignalId { get; set; }
    
    [Required]
    public DateTime ExecutionTime { get; set; }
    
    [Required]
    public Price ExecutionPrice { get; set; } = null!;
    
    [Required]
    public Volume ExecutedQuantity { get; set; }
    
    [MaxLength(50)]
    public string? ExecutionVenue { get; set; }  // Onde foi executado
    
    [MaxLength(100)]
    public string? OrderId { get; set; }  // ID da ordem no broker
    
    public decimal? Slippage { get; set; }  // Diferença entre preço esperado e executado
    public decimal? Commission { get; set; }  // Comissão paga
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    // Relacionamentos
    public virtual TradingSignal Signal { get; set; } = null!;
}



