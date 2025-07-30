namespace BotSinais.Domain.Shared.ValueObjects;

/// <summary>
/// Value Object para representar volume
/// </summary>
public record Volume
{
    public long Value { get; init; }
    
    public Volume(long value)
    {
        if (value < 0)
            throw new ArgumentException("Volume cannot be negative", nameof(value));
        
        Value = value;
    }
    
    public static implicit operator long(Volume volume) => volume.Value;
    public static implicit operator Volume(long value) => new(value);
    
    public override string ToString() => Value.ToString("N0");
}

