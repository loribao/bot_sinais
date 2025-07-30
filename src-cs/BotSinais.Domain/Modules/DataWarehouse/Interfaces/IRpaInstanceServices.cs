using BotSinais.Domain.Modules.DataWarehouse.Entities;

namespace BotSinais.Domain.Modules.DataWarehouse.Interfaces;

/// <summary>
/// Repositório para gerenciamento de instâncias de RPA
/// </summary>
public interface IRpaInstanceRepository
{
    // CRUD básico
    Task<RpaInstance?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RpaInstance?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<RpaInstance>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RpaInstance> CreateAsync(RpaInstance rpaInstance, CancellationToken cancellationToken = default);
    Task<RpaInstance> UpdateAsync(RpaInstance rpaInstance, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    // Consultas específicas
    Task<IEnumerable<RpaInstance>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IEnumerable<RpaInstance>> GetOnlineInstancesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RpaInstance>> GetByTypeAsync(string rpaType, CancellationToken cancellationToken = default);
    Task<IEnumerable<RpaInstance>> GetByDataSourceAsync(string dataSource, CancellationToken cancellationToken = default);
    Task<IEnumerable<RpaInstance>> GetAvailableForRequestsAsync(CancellationToken cancellationToken = default);
    
    // Health checks
    Task<IEnumerable<RpaHealthCheck>> GetHealthCheckHistoryAsync(
        Guid rpaInstanceId, 
        DateTime? from = null, 
        DateTime? to = null,
        CancellationToken cancellationToken = default);
    
    Task<RpaHealthCheck> CreateHealthCheckAsync(RpaHealthCheck healthCheck, CancellationToken cancellationToken = default);
    
    // Métricas
    Task<Dictionary<string, object>> GetInstanceMetricsAsync(
        Guid rpaInstanceId, 
        TimeSpan period,
        CancellationToken cancellationToken = default);
    
    Task<Dictionary<string, object>> GetOverallMetricsAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Serviço de gerenciamento de instâncias de RPA
/// </summary>
public interface IRpaInstanceManagementService
{
    // Registro e configuração
    Task<RpaInstance> RegisterRpaAsync(
        string name,
        string rpaType,
        string dataSource,
        string connectionString,
        string? description = null,
        Dictionary<string, object>? configuration = null,
        CancellationToken cancellationToken = default);
    
    Task<RpaInstance> UpdateRpaAsync(
        Guid rpaInstanceId,
        string? name = null,
        string? description = null,
        string? connectionString = null,
        Dictionary<string, object>? configuration = null,
        CancellationToken cancellationToken = default);
    
    Task<bool> UnregisterRpaAsync(Guid rpaInstanceId, CancellationToken cancellationToken = default);
    
    // Controle de status
    Task<bool> SetRpaStatusAsync(Guid rpaInstanceId, string status, string? reason = null, CancellationToken cancellationToken = default);
    Task<bool> MarkRpaOnlineAsync(Guid rpaInstanceId, CancellationToken cancellationToken = default);
    Task<bool> MarkRpaOfflineAsync(Guid rpaInstanceId, string? reason = null, CancellationToken cancellationToken = default);
    Task<bool> SetMaintenanceModeAsync(Guid rpaInstanceId, bool maintenanceMode, CancellationToken cancellationToken = default);
    
    // Heartbeat e health checks
    Task<bool> ProcessHeartbeatAsync(Guid rpaInstanceId, Dictionary<string, object>? metrics = null, CancellationToken cancellationToken = default);
    Task<bool> PerformHealthCheckAsync(Guid rpaInstanceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RpaInstance>> GetUnhealthyInstancesAsync(CancellationToken cancellationToken = default);
    
    // Seleção de RPAs para execução
    Task<RpaInstance?> SelectBestRpaForRequestAsync(
        string rpaType,
        string dataSource,
        int priority = 5,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<RpaInstance>> GetAvailableRpasAsync(
        string? rpaType = null,
        string? dataSource = null,
        CancellationToken cancellationToken = default);
    
    // Monitoramento
    Task<RpaInstanceStatus> GetInstanceStatusAsync(Guid rpaInstanceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RpaInstanceStatus>> GetAllInstancesStatusAsync(CancellationToken cancellationToken = default);
    Task<RpaClusterMetrics> GetClusterMetricsAsync(CancellationToken cancellationToken = default);
    
    // Limpeza e manutenção
    Task<int> CleanupOldHealthChecksAsync(TimeSpan retention, CancellationToken cancellationToken = default);
    Task<int> ResetOfflineInstancesAsync(TimeSpan offlineThreshold, CancellationToken cancellationToken = default);
}

/// <summary>
/// Serviço de monitoramento de RPAs
/// </summary>
public interface IRpaMonitoringService
{
    // Verificação de saúde contínua
    Task StartMonitoringAsync(CancellationToken cancellationToken = default);
    Task StopMonitoringAsync(CancellationToken cancellationToken = default);
    
    // Alertas
    Task<IEnumerable<RpaAlert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default);
    Task<bool> AcknowledgeAlertAsync(Guid alertId, string acknowledgedBy, CancellationToken cancellationToken = default);
    
    // Relatórios
    Task<RpaPerformanceReport> GeneratePerformanceReportAsync(
        Guid? rpaInstanceId = null,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default);
    
    Task<RpaAvailabilityReport> GenerateAvailabilityReportAsync(
        TimeSpan period,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Status detalhado de uma instância RPA
/// </summary>
public class RpaInstanceStatus
{
    public Guid RpaInstanceId { get; set; }
    public string Name { get; set; } = null!;
    public string RpaType { get; set; } = null!;
    public string DataSource { get; set; } = null!;
    public string Status { get; set; } = null!;
    public bool IsOnline { get; set; }
    public bool CanAcceptRequests { get; set; }
    public int ActiveRequests { get; set; }
    public int MaxConcurrentRequests { get; set; }
    public decimal SuccessRate { get; set; }
    public TimeSpan? LastHeartbeatAge { get; set; }
    public string? LastError { get; set; }
    public DateTime? LastErrorAt { get; set; }
    public Dictionary<string, object> Metrics { get; set; } = new();
}

/// <summary>
/// Métricas do cluster de RPAs
/// </summary>
public class RpaClusterMetrics
{
    public int TotalInstances { get; set; }
    public int OnlineInstances { get; set; }
    public int OfflineInstances { get; set; }
    public int BusyInstances { get; set; }
    public int ErrorInstances { get; set; }
    public int MaintenanceInstances { get; set; }
    public decimal OverallSuccessRate { get; set; }
    public decimal AverageResponseTime { get; set; }
    public long TotalRequestsProcessed { get; set; }
    public long TotalErrors { get; set; }
    public Dictionary<string, int> InstancesByType { get; set; } = new();
    public Dictionary<string, int> InstancesByDataSource { get; set; } = new();
}

/// <summary>
/// Alerta de RPA
/// </summary>
public class RpaAlert
{
    public Guid Id { get; set; }
    public Guid RpaInstanceId { get; set; }
    public string AlertType { get; set; } = null!; // "Offline", "HighErrorRate", "Timeout", "Maintenance"
    public string Severity { get; set; } = null!; // "Low", "Medium", "High", "Critical"
    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsAcknowledged { get; set; }
    public string? AcknowledgedBy { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
}

/// <summary>
/// Relatório de performance de RPAs
/// </summary>
public class RpaPerformanceReport
{
    public DateTime GeneratedAt { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Guid? RpaInstanceId { get; set; }
    public string? RpaInstanceName { get; set; }
    public long TotalRequests { get; set; }
    public long SuccessfulRequests { get; set; }
    public long FailedRequests { get; set; }
    public decimal SuccessRate { get; set; }
    public decimal AverageResponseTime { get; set; }
    public decimal MinResponseTime { get; set; }
    public decimal MaxResponseTime { get; set; }
    public long TotalDataProcessed { get; set; }
    public Dictionary<string, long> ErrorsByType { get; set; } = new();
    public Dictionary<DateTime, decimal> ResponseTimeByHour { get; set; } = new();
}

/// <summary>
/// Relatório de disponibilidade de RPAs
/// </summary>
public class RpaAvailabilityReport
{
    public DateTime GeneratedAt { get; set; }
    public TimeSpan Period { get; set; }
    public decimal OverallAvailability { get; set; }
    public List<RpaInstanceAvailability> InstancesAvailability { get; set; } = new();
    public Dictionary<DateTime, int> OnlineInstancesByHour { get; set; } = new();
}

/// <summary>
/// Disponibilidade de uma instância específica
/// </summary>
public class RpaInstanceAvailability
{
    public Guid RpaInstanceId { get; set; }
    public string Name { get; set; } = null!;
    public string RpaType { get; set; } = null!;
    public string DataSource { get; set; } = null!;
    public decimal AvailabilityPercent { get; set; }
    public TimeSpan TotalUptime { get; set; }
    public TimeSpan TotalDowntime { get; set; }
    public int DowntimeIncidents { get; set; }
    public TimeSpan AverageDowntimeDuration { get; set; }
}
