using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using BotSinais.Domain.Shared.Entities;

namespace BotSinais.Domain.Modules.DataWarehouse.Entities;

/// <summary>
/// Configuração de RPA para coleta de dados
/// </summary>
public class RpaConfiguration : BaseEntity
{
    [Required]
    public Guid RpaInstanceId { get; set; } // Referência ao RPA registrado
    
    [Required]
    [MaxLength(100)]
    public string RpaType { get; set; } = null!; // "MarketData", "OnChain", "News", "Social"
    
    [Required]
    [MaxLength(100)]
    public string DataSource { get; set; } = null!; // "Binance", "Coinbase", "Etherscan", "Twitter"
    
    [Required]
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    [Required]
    public new bool IsActive { get; set; } = true;
    
    [Required]
    public TimeSpan CollectionInterval { get; set; } = TimeSpan.FromMinutes(1);
    
    [Required]
    public int Priority { get; set; } = 5; // 1-10
    
    [Required]
    public int MaxRetryAttempts { get; set; } = 3;
    
    [Required]
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(30);
    
    [Required]
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(30);
    
    // Configurações específicas em JSON
    public string ConfigurationJson { get; set; } = "{}";
    
    // Credenciais (referência externa por segurança)
    public string? CredentialsReference { get; set; }
    
    // Monitoramento
    public DateTime? LastExecuted { get; set; }
    public DateTime? LastSuccessful { get; set; }
    public int ConsecutiveFailures { get; set; }
    public string? LastErrorMessage { get; set; }
    
    // Relacionamentos
    public virtual RpaInstance RpaInstance { get; set; } = null!;
    public virtual ICollection<RpaDataRequest> DataRequests { get; set; } = new List<RpaDataRequest>();
}

/// <summary>
/// Solicitação de coleta de dados para RPA
/// </summary>
public class RpaDataRequest : BaseEntity
{
    [Required]
    public Guid ConfigurationId { get; set; }
    
    [Required]
    public Guid RequestId { get; set; } // Para tracking via eventos
    
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Started, Running, Completed, Failed, Cancelled
    
    public Symbol? Symbol { get; set; }
    public TimeFrame? TimeFrame { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    [Required]
    public int Priority { get; set; } = 5;
    
    // Parâmetros específicos da solicitação
    public string ParametersJson { get; set; } = "{}";
    
    // Tracking
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? LastActivityAt { get; set; }
    
    // Resultado
    public int RecordsProcessed { get; set; }
    public long DataSizeBytes { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorDetails { get; set; }
    public int AttemptNumber { get; set; } = 1;
    
    // MongoDB Info
    public string? DatabaseName { get; set; }
    public string? CollectionName { get; set; }
    public string? DataQualityScore { get; set; }
    
    // Relacionamentos
    public virtual RpaConfiguration Configuration { get; set; } = null!;
    public virtual ICollection<RpaDataBatch> DataBatches { get; set; } = new List<RpaDataBatch>();
}

/// <summary>
/// Batch de dados coletados pelo RPA
/// </summary>
public class RpaDataBatch : BaseEntity
{
    [Required]
    public Guid RequestId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string BatchNumber { get; set; } = null!;
    
    [Required]
    public DateTime DataTimestamp { get; set; }
    
    [Required]
    public int RecordCount { get; set; }
    
    [Required]
    public long SizeBytes { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string DatabaseName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string CollectionName { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string DataType { get; set; } = null!; // "MarketData", "OnChain", "News", etc.
    
    public Symbol? Symbol { get; set; }
    public TimeFrame? TimeFrame { get; set; }
    
    // Schema e metadados
    public string SchemaJson { get; set; } = "{}";
    public string MetadataJson { get; set; } = "{}";
    
    // Qualidade dos dados
    [Required]
    [MaxLength(20)]
    public string QualityScore { get; set; } = "Unknown"; // High, Medium, Low, Unknown
    
    public decimal? CompletenessPercent { get; set; }
    public decimal? AccuracyScore { get; set; }
    
    // Status de processamento
    [Required]
    [MaxLength(50)]
    public string ProcessingStatus { get; set; } = "Pending"; // Pending, Processing, Processed, Failed
    
    public DateTime? ProcessedAt { get; set; }
    public string? ProcessingErrorMessage { get; set; }
    
    // Relacionamentos
    public virtual RpaDataRequest Request { get; set; } = null!;
}

/// <summary>
/// Log de atividades do RPA
/// </summary>
public class RpaActivityLog : BaseEntity
{
    [Required]
    public Guid RequestId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string RpaType { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string DataSource { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string ActivityType { get; set; } = null!; // "Started", "Progress", "Completed", "Error", "Warning"
    
    [Required]
    public string Message { get; set; } = null!;
    
    public string? Details { get; set; }
    
    // Contexto da atividade
    public string ContextJson { get; set; } = "{}";
    
    // Métricas
    public int? RecordsProcessed { get; set; }
    public long? DataSizeBytes { get; set; }
    public TimeSpan? Duration { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
