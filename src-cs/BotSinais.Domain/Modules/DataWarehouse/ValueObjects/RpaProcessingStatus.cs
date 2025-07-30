namespace BotSinais.Domain.Modules.DataWarehouse.ValueObjects;

/// <summary>
/// Representa status de processamento RPA
/// </summary>
public record RpaProcessingStatus
{
    public string Status { get; init; } = null!; // Pending, Started, Running, Completed, Failed, Stopped
    public string? Message { get; init; }
    public decimal ProgressPercent { get; init; }
    public DateTime LastActivity { get; init; }
    public TimeSpan? EstimatedTimeRemaining { get; init; }
    public Dictionary<string, object> Metadata { get; init; } = new();

    public static RpaProcessingStatus Pending(string? message = null) => new()
    {
        Status = "Pending",
        Message = message ?? "Waiting to start",
        ProgressPercent = 0,
        LastActivity = DateTime.UtcNow
    };

    public static RpaProcessingStatus Started(string? message = null) => new()
    {
        Status = "Started",
        Message = message ?? "Processing started",
        ProgressPercent = 0,
        LastActivity = DateTime.UtcNow
    };

    public static RpaProcessingStatus Running(decimal progressPercent, string? message = null, TimeSpan? eta = null) => new()
    {
        Status = "Running",
        Message = message ?? $"Processing {progressPercent:F1}% complete",
        ProgressPercent = Math.Clamp(progressPercent, 0, 100),
        LastActivity = DateTime.UtcNow,
        EstimatedTimeRemaining = eta
    };

    public static RpaProcessingStatus Completed(string? message = null) => new()
    {
        Status = "Completed",
        Message = message ?? "Processing completed successfully",
        ProgressPercent = 100,
        LastActivity = DateTime.UtcNow
    };

    public static RpaProcessingStatus Failed(string errorMessage) => new()
    {
        Status = "Failed",
        Message = errorMessage,
        ProgressPercent = 0,
        LastActivity = DateTime.UtcNow
    };

    public static RpaProcessingStatus Stopped(string? reason = null) => new()
    {
        Status = "Stopped",
        Message = reason ?? "Processing stopped",
        LastActivity = DateTime.UtcNow
    };

    public bool IsActive => Status is "Started" or "Running";
    public bool IsCompleted => Status is "Completed" or "Failed" or "Stopped";
}
