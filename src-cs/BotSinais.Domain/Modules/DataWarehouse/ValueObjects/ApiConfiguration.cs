namespace BotSinais.Domain.Modules.DataWarehouse.ValueObjects;

/// <summary>
/// Representa configurações de API para RPAs
/// </summary>
public record ApiConfiguration
{
    public string BaseUrl { get; init; } = null!;
    public string CredentialsReference { get; init; } = null!;
    public int RateLimitPerMinute { get; init; } = 1200;
    public TimeSpan RequestTimeout { get; init; } = TimeSpan.FromSeconds(30);
    public int MaxRetryAttempts { get; init; } = 3;
    public TimeSpan RetryDelay { get; init; } = TimeSpan.FromSeconds(30);
    public Dictionary<string, string> Headers { get; init; } = new();
    public Dictionary<string, object> Parameters { get; init; } = new();

    public static ApiConfiguration Create(
        string baseUrl,
        string credentialsReference,
        int rateLimitPerMinute = 1200)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("Base URL cannot be empty", nameof(baseUrl));
        
        if (string.IsNullOrWhiteSpace(credentialsReference))
            throw new ArgumentException("Credentials reference cannot be empty", nameof(credentialsReference));
        
        if (rateLimitPerMinute <= 0)
            throw new ArgumentException("Rate limit must be positive", nameof(rateLimitPerMinute));

        if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out _))
            throw new ArgumentException("Base URL must be a valid URI", nameof(baseUrl));

        return new ApiConfiguration
        {
            BaseUrl = baseUrl,
            CredentialsReference = credentialsReference,
            RateLimitPerMinute = rateLimitPerMinute
        };
    }
}
