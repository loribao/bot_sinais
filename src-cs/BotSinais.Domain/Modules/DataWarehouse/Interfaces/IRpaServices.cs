using BotSinais.Domain.Modules.DataWarehouse.Events;
using BotSinais.Domain.Modules.DataWarehouse.Entities;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Shared.Enums;

namespace BotSinais.Domain.Modules.DataWarehouse.Interfaces;

/// <summary>
/// Interface para gerenciamento de RPAs
/// </summary>
public interface IRpaManagementService
{
    // Gerenciamento de configurações
    Task<RpaConfiguration> CreateConfigurationAsync(
        string rpaType, 
        string dataSource, 
        string name,
        Dictionary<string, object> configuration,
        CancellationToken cancellationToken = default);
    
    Task<RpaConfiguration> UpdateConfigurationAsync(
        Guid configurationId,
        Dictionary<string, object> configuration,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<RpaConfiguration>> GetActiveConfigurationsAsync(
        string? rpaType = null,
        CancellationToken cancellationToken = default);
    
    // Solicitações de dados
    Task<RpaDataRequest> RequestDataCollectionAsync(
        Guid configurationId,
        Symbol? symbol = null,
        TimeFrame? timeFrame = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        Dictionary<string, object>? parameters = null,
        int priority = 5,
        CancellationToken cancellationToken = default);
    
    Task<bool> CancelDataRequestAsync(
        Guid requestId,
        string reason = "Manual cancellation",
        CancellationToken cancellationToken = default);
    
    // Monitoramento
    Task<IEnumerable<RpaDataRequest>> GetActiveRequestsAsync(
        string? rpaType = null,
        CancellationToken cancellationToken = default);
    
    Task<RpaDataRequest?> GetRequestStatusAsync(
        Guid requestId,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<RpaActivityLog>> GetActivityLogsAsync(
        Guid requestId,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface para comunicação via mensageria com RPAs
/// </summary>
public interface IRpaMessagingService
{
    // Envio de comandos
    Task SendStartCollectionCommandAsync(
        StartDataCollectionCommand command,
        CancellationToken cancellationToken = default);
    
    Task SendStopCollectionCommandAsync(
        StopDataCollectionCommand command,
        CancellationToken cancellationToken = default);
    
    Task SendConfigurationCommandAsync(
        ConfigureDataCollectionCommand command,
        CancellationToken cancellationToken = default);
    
    // Handlers de eventos (implementados pela Infrastructure)
    Task HandleDataCollectionStatusAsync(
        DataCollectionStatusEvent statusEvent,
        CancellationToken cancellationToken = default);
    
    Task HandleDataAvailableAsync(
        DataAvailableEvent dataEvent,
        CancellationToken cancellationToken = default);
    
    Task HandleRpaErrorAsync(
        RpaErrorEvent errorEvent,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface para acesso aos dados no MongoDB
/// </summary>
public interface IMongoDataAccessService
{
    // Verificação de dados disponíveis
    Task<bool> IsDataAvailableAsync(
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default);
    
    Task<long> GetRecordCountAsync(
        string databaseName,
        string collectionName,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);
    
    // Leitura de dados (genérica)
    Task<IEnumerable<T>> GetDataAsync<T>(
        string databaseName,
        string collectionName,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? limit = null,
        CancellationToken cancellationToken = default);
    
    // Leitura de dados raw (como documentos)
    Task<IEnumerable<Dictionary<string, object>>> GetRawDataAsync(
        string databaseName,
        string collectionName,
        Dictionary<string, object>? filter = null,
        int? limit = null,
        CancellationToken cancellationToken = default);
    
    // Agregações para análise
    Task<IEnumerable<Dictionary<string, object>>> AggregateAsync(
        string databaseName,
        string collectionName,
        string[] pipeline,
        CancellationToken cancellationToken = default);
    
    // Metadados
    Task<Dictionary<string, object>> GetCollectionSchemaAsync(
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default);
    
    Task<Dictionary<string, object>> GetCollectionStatsAsync(
        string databaseName,
        string collectionName,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface para processamento de dados do MongoDB para o Data Warehouse
/// </summary>
public interface IDataProcessingService
{
    // Processamento de dados de mercado
    Task<int> ProcessMarketDataAsync(
        string databaseName,
        string collectionName,
        Symbol symbol,
        TimeFrame timeFrame,
        CancellationToken cancellationToken = default);
    
    // Processamento de dados on-chain
    Task<int> ProcessOnChainDataAsync(
        string databaseName,
        string collectionName,
        Symbol symbol,
        CancellationToken cancellationToken = default);
    
    // Processamento de notícias/sentiment
    Task<int> ProcessNewsDataAsync(
        string databaseName,
        string collectionName,
        Symbol? symbol = null,
        CancellationToken cancellationToken = default);
    
    // Validação de qualidade
    Task<DataQualityReport> ValidateDataQualityAsync(
        string databaseName,
        string collectionName,
        string dataType,
        CancellationToken cancellationToken = default);
    
    // Limpeza de dados antigos
    Task<int> CleanupOldDataAsync(
        string databaseName,
        string collectionName,
        DateTime cutoffDate,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Repositório para entidades de RPA
/// </summary>
public interface IRpaRepository
{
    // RpaConfiguration
    Task<RpaConfiguration?> GetConfigurationByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<RpaConfiguration>> GetConfigurationsByTypeAsync(
        string rpaType,
        CancellationToken cancellationToken = default);
    
    Task<RpaConfiguration> CreateConfigurationAsync(
        RpaConfiguration configuration,
        CancellationToken cancellationToken = default);
    
    Task<RpaConfiguration> UpdateConfigurationAsync(
        RpaConfiguration configuration,
        CancellationToken cancellationToken = default);
    
    // RpaDataRequest
    Task<RpaDataRequest?> GetRequestByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<RpaDataRequest?> GetRequestByRequestIdAsync(
        Guid requestId,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<RpaDataRequest>> GetActiveRequestsAsync(
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<RpaDataRequest>> GetRequestsByStatusAsync(
        string status,
        CancellationToken cancellationToken = default);
    
    Task<RpaDataRequest> CreateRequestAsync(
        RpaDataRequest request,
        CancellationToken cancellationToken = default);
    
    Task<RpaDataRequest> UpdateRequestAsync(
        RpaDataRequest request,
        CancellationToken cancellationToken = default);
    
    // RpaDataBatch
    Task<IEnumerable<RpaDataBatch>> GetBatchesByRequestAsync(
        Guid requestId,
        CancellationToken cancellationToken = default);
    
    Task<RpaDataBatch> CreateBatchAsync(
        RpaDataBatch batch,
        CancellationToken cancellationToken = default);
    
    Task<RpaDataBatch> UpdateBatchAsync(
        RpaDataBatch batch,
        CancellationToken cancellationToken = default);
    
    // RpaActivityLog
    Task<IEnumerable<RpaActivityLog>> GetActivityLogsByRequestAsync(
        Guid requestId,
        CancellationToken cancellationToken = default);
    
    Task<RpaActivityLog> CreateActivityLogAsync(
        RpaActivityLog log,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Model para relatório de qualidade de dados
/// </summary>
public class DataQualityReport
{
    public string DatabaseName { get; set; } = null!;
    public string CollectionName { get; set; } = null!;
    public string DataType { get; set; } = null!;
    public long TotalRecords { get; set; }
    public decimal CompletenessPercent { get; set; }
    public decimal AccuracyScore { get; set; }
    public string QualityLevel { get; set; } = "Unknown"; // High, Medium, Low
    public List<string> Issues { get; set; } = new();
    public Dictionary<string, object> Metrics { get; set; } = new();
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}
