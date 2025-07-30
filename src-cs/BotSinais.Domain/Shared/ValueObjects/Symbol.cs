namespace BotSinais.Domain.Shared.ValueObjects;

/// <summary>
/// Value Object para representar um símbolo de trading
/// </summary>
public record Symbol
{
    public string Code { get; init; }
    public string Exchange { get; init; }
    
    public Symbol(string code, string exchange)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Symbol code cannot be null or empty", nameof(code));
        
        if (string.IsNullOrWhiteSpace(exchange))
            throw new ArgumentException("Exchange cannot be null or empty", nameof(exchange));
        
        Code = code.ToUpperInvariant();
        Exchange = exchange.ToUpperInvariant();
    }
    
    public override string ToString() => $"{Code}@{Exchange}";
}

