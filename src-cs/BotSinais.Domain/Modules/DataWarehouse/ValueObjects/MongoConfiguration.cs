using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Modules.DataWarehouse.ValueObjects;

/// <summary>
/// Representa uma configuração MongoDB para RPAs
/// </summary>
public record MongoConfiguration
{
    public string DatabaseName { get; init; } = null!;
    public string CollectionTemplate { get; init; } = null!; // e.g., "{exchange}_{symbol}_{timeframe}"
    public string ConnectionString { get; init; } = null!;
    public int BatchSize { get; init; } = 1000;
    public TimeSpan WriteTimeout { get; init; } = TimeSpan.FromSeconds(30);
    public bool EnableRetryWrites { get; init; } = true;
    public Dictionary<string, object> IndexDefinitions { get; init; } = new();

    public static MongoConfiguration Create(
        string databaseName, 
        string collectionTemplate,
        string connectionString,
        int batchSize = 1000)
    {
        if (string.IsNullOrWhiteSpace(databaseName))
            throw new ArgumentException("Database name cannot be empty", nameof(databaseName));
        
        if (string.IsNullOrWhiteSpace(collectionTemplate))
            throw new ArgumentException("Collection template cannot be empty", nameof(collectionTemplate));
        
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be empty", nameof(connectionString));
        
        if (batchSize <= 0)
            throw new ArgumentException("Batch size must be positive", nameof(batchSize));

        return new MongoConfiguration
        {
            DatabaseName = databaseName,
            CollectionTemplate = collectionTemplate,
            ConnectionString = connectionString,
            BatchSize = batchSize
        };
    }
}
