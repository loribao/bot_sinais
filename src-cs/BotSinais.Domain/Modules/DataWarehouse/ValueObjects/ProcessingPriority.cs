namespace BotSinais.Domain.Modules.DataWarehouse.ValueObjects;

/// <summary>
/// Representa prioridade de processamento
/// </summary>
public record ProcessingPriority
{
    public int Level { get; init; } // 1-10, onde 10 é máxima prioridade
    public string Description { get; init; } = null!;
    public TimeSpan MaxWaitTime { get; init; }

    public static ProcessingPriority Critical => new() { Level = 10, Description = "Critical", MaxWaitTime = TimeSpan.Zero };
    public static ProcessingPriority High => new() { Level = 8, Description = "High", MaxWaitTime = TimeSpan.FromMinutes(1) };
    public static ProcessingPriority Normal => new() { Level = 5, Description = "Normal", MaxWaitTime = TimeSpan.FromMinutes(5) };
    public static ProcessingPriority Low => new() { Level = 2, Description = "Low", MaxWaitTime = TimeSpan.FromMinutes(30) };

    public static ProcessingPriority FromLevel(int level)
    {
        if (level < 1 || level > 10)
            throw new ArgumentException("Priority level must be between 1 and 10", nameof(level));

        return level switch
        {
            >= 9 => Critical,
            >= 7 => High,
            >= 4 => Normal,
            _ => Low
        };
    }
}
