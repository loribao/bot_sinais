using BotSinais.Domain.Shared.Entities;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.DataWarehouse.Entities;

/// <summary>
/// Entidade que representa um RPA registrado no sistema
/// </summary>
public class RpaInstance : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string RpaType { get; set; } = null!; // "MarketData", "OnChain", "News", "Social"
    
    [Required]
    [MaxLength(100)]
    public string DataSource { get; set; } = null!; // "Binance", "Coinbase", "Etherscan", "Twitter"
    
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Offline"; // "Online", "Offline", "Busy", "Error", "Maintenance"
    
    [Required]
    public string Version { get; set; } = "1.0.0";
    
    // Informações de conexão
    [Required]
    [MaxLength(200)]
    public string ConnectionString { get; set; } = null!; // Para mensageria/endpoint
    
    public string? IpAddress { get; set; }
    public int? Port { get; set; }
    
    // Capacidades do RPA
    public int MaxConcurrentRequests { get; set; } = 1;
    public int CurrentActiveRequests { get; set; } = 0;
    
    // Métricas de performance
    public decimal AverageResponseTimeMs { get; set; } = 0;
    public decimal SuccessRate { get; set; } = 100; // Percentual
    public long TotalRequestsProcessed { get; set; } = 0;
    public long TotalErrorsCount { get; set; } = 0;
    
    // Status de saúde
    public DateTime? LastHeartbeat { get; set; }
    public DateTime? LastSuccessfulRequest { get; set; }
    public string? LastErrorMessage { get; set; }
    public DateTime? LastErrorAt { get; set; }
    
    // Configurações de monitoramento
    [Required]
    public TimeSpan HeartbeatInterval { get; set; } = TimeSpan.FromMinutes(1);
    
    [Required]
    public TimeSpan HealthCheckTimeout { get; set; } = TimeSpan.FromSeconds(30);
    
    // Configurações específicas em JSON
    public string ConfigurationJson { get; set; } = "{}";
    
    // Relacionamentos
    public virtual ICollection<RpaConfiguration> Configurations { get; set; } = new List<RpaConfiguration>();
    public virtual ICollection<RpaHealthCheck> HealthChecks { get; set; } = new List<RpaHealthCheck>();
    
    // Métodos de domínio
    public bool IsOnline => Status == "Online" && 
                           LastHeartbeat.HasValue && 
                           DateTime.UtcNow - LastHeartbeat.Value < HeartbeatInterval.Add(TimeSpan.FromMinutes(1));
    
    public bool CanAcceptNewRequest => IsOnline && 
                                     CurrentActiveRequests < MaxConcurrentRequests && 
                                     Status != "Maintenance";
    
    public void UpdateHeartbeat()
    {
        LastHeartbeat = DateTime.UtcNow;
        if (Status == "Offline")
        {
            Status = "Online";
        }
    }
    
    public void MarkAsOffline(string? reason = null)
    {
        Status = "Offline";
        if (!string.IsNullOrEmpty(reason))
        {
            LastErrorMessage = reason;
            LastErrorAt = DateTime.UtcNow;
        }
    }
    
    public void IncrementActiveRequests()
    {
        CurrentActiveRequests++;
        if (CurrentActiveRequests >= MaxConcurrentRequests)
        {
            Status = "Busy";
        }
    }
    
    public void DecrementActiveRequests()
    {
        if (CurrentActiveRequests > 0)
        {
            CurrentActiveRequests--;
        }
        
        if (Status == "Busy" && CurrentActiveRequests < MaxConcurrentRequests)
        {
            Status = "Online";
        }
    }
    
    public void RecordSuccessfulRequest(TimeSpan responseTime)
    {
        TotalRequestsProcessed++;
        LastSuccessfulRequest = DateTime.UtcNow;
        
        // Atualizar média de tempo de resposta (média móvel simples)
        AverageResponseTimeMs = (AverageResponseTimeMs * 0.9m) + ((decimal)responseTime.TotalMilliseconds * 0.1m);
        
        // Atualizar taxa de sucesso
        SuccessRate = TotalRequestsProcessed > 0 ? 
            ((decimal)(TotalRequestsProcessed - TotalErrorsCount) / TotalRequestsProcessed) * 100 : 100;
    }
    
    public void RecordError(string errorMessage)
    {
        TotalErrorsCount++;
        LastErrorMessage = errorMessage;
        LastErrorAt = DateTime.UtcNow;
        
        // Atualizar taxa de sucesso
        SuccessRate = TotalRequestsProcessed > 0 ? 
            ((decimal)(TotalRequestsProcessed - TotalErrorsCount) / TotalRequestsProcessed) * 100 : 0;
            
        // Se muitos erros consecutivos, marcar como erro
        if (TotalErrorsCount > 0 && SuccessRate < 50 && Status == "Online")
        {
            Status = "Error";
        }
    }
}

/// <summary>
/// Health check de um RPA
/// </summary>
public class RpaHealthCheck : BaseEntity
{
    [Required]
    public Guid RpaInstanceId { get; set; }
    
    [Required]
    public DateTime CheckTimestamp { get; set; } = DateTime.UtcNow;
    
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = null!; // "Healthy", "Unhealthy", "Timeout", "Error"
    
    public TimeSpan ResponseTime { get; set; }
    
    public string? ErrorMessage { get; set; }
    public string? ErrorDetails { get; set; }
    
    // Métricas coletadas durante o health check
    public decimal? CpuUsagePercent { get; set; }
    public decimal? MemoryUsageMb { get; set; }
    public int? ActiveConnections { get; set; }
    public int? QueuedRequests { get; set; }
    
    // Informações adicionais em JSON
    public string MetricsJson { get; set; } = "{}";
    
    // Relacionamentos
    public virtual RpaInstance RpaInstance { get; set; } = null!;
}
