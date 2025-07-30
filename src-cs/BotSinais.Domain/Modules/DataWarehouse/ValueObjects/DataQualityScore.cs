namespace BotSinais.Domain.Modules.DataWarehouse.ValueObjects;

/// <summary>
/// Representa um score de qualidade de dados
/// </summary>
public record DataQualityScore
{
    public decimal CompletenessPercent { get; init; }
    public decimal AccuracyPercent { get; init; }
    public decimal TimelinessScore { get; init; } // 0-1, onde 1 é tempo real
    public decimal ConsistencyScore { get; init; } // 0-1
    public decimal UniquenessPercent { get; init; }
    public decimal FinalScore { get; init; }
    public string QualityLevel { get; init; } = null!; // High, Medium, Low

    public static DataQualityScore Calculate(
        decimal completeness,
        decimal accuracy,
        decimal timeliness,
        decimal consistency,
        decimal uniqueness)
    {
        // Validações
        ValidatePercentage(completeness, nameof(completeness));
        ValidatePercentage(accuracy, nameof(accuracy));
        ValidateScore(timeliness, nameof(timeliness));
        ValidateScore(consistency, nameof(consistency));
        ValidatePercentage(uniqueness, nameof(uniqueness));

        // Cálculo do score final (média ponderada)
        var finalScore = (completeness * 0.3m) + 
                        (accuracy * 0.3m) + 
                        (timeliness * 0.2m) + 
                        (consistency * 0.15m) + 
                        (uniqueness * 0.05m);

        var qualityLevel = finalScore switch
        {
            >= 90m => "High",
            >= 70m => "Medium",
            _ => "Low"
        };

        return new DataQualityScore
        {
            CompletenessPercent = completeness,
            AccuracyPercent = accuracy,
            TimelinessScore = timeliness,
            ConsistencyScore = consistency,
            UniquenessPercent = uniqueness,
            FinalScore = finalScore,
            QualityLevel = qualityLevel
        };
    }

    private static void ValidatePercentage(decimal value, string paramName)
    {
        if (value < 0 || value > 100)
            throw new ArgumentException($"Percentage must be between 0 and 100", paramName);
    }

    private static void ValidateScore(decimal value, string paramName)
    {
        if (value < 0 || value > 1)
            throw new ArgumentException($"Score must be between 0 and 1", paramName);
    }
}
