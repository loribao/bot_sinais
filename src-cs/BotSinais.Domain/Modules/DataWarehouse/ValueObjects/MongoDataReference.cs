using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Modules.DataWarehouse.ValueObjects;

/// <summary>
/// Representa uma referência a dados no MongoDB
/// </summary>
public record MongoDataReference
{
    public string DatabaseName { get; init; } = null!;
    public string CollectionName { get; init; } = null!;
    public DateTime DataTimestamp { get; init; }
    public Symbol? Symbol { get; init; }
    public string DataType { get; init; } = null!; // MarketData, OnChain, News, etc.
    public long RecordCount { get; init; }
    public long SizeBytes { get; init; }
    public Dictionary<string, object> Schema { get; init; } = new();

    public static MongoDataReference Create(
        string databaseName,
        string collectionName,
        DateTime dataTimestamp,
        string dataType,
        long recordCount,
        long sizeBytes)
    {
        if (string.IsNullOrWhiteSpace(databaseName))
            throw new ArgumentException("Database name cannot be empty", nameof(databaseName));
        
        if (string.IsNullOrWhiteSpace(collectionName))
            throw new ArgumentException("Collection name cannot be empty", nameof(collectionName));
        
        if (string.IsNullOrWhiteSpace(dataType))
            throw new ArgumentException("Data type cannot be empty", nameof(dataType));
        
        if (recordCount < 0)
            throw new ArgumentException("Record count cannot be negative", nameof(recordCount));
        
        if (sizeBytes < 0)
            throw new ArgumentException("Size bytes cannot be negative", nameof(sizeBytes));

        return new MongoDataReference
        {
            DatabaseName = databaseName,
            CollectionName = collectionName,
            DataTimestamp = dataTimestamp,
            DataType = dataType,
            RecordCount = recordCount,
            SizeBytes = sizeBytes
        };
    }

    public string GetFullPath() => $"{DatabaseName}.{CollectionName}";
}
