using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Modules.DataManagement.Entities;

/// <summary>
/// Entidade que representa dados de mercado (OHLCV)
/// </summary>
public class MarketData : BaseEntity
{
    [Required]
    public Guid InstrumentId { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; }
    
    [Required]
    public TimeFrame TimeFrame { get; set; }
    
    public Price Open { get; set; } = null!;
    public Price High { get; set; } = null!;
    public Price Low { get; set; } = null!;
    public Price Close { get; set; } = null!;
    public Volume Volume { get; set; }
    
    // Dados adicionais
    public Price? VWAP { get; set; }  // Volume Weighted Average Price
    public long? Trades { get; set; }  // Número de trades
    public decimal? Spread { get; set; }  // Spread bid/ask
    
    // Relacionamentos
    public virtual Instrument Instrument { get; set; } = null!;
    
    // Índices únicos
    public static class Indexes
    {
        public const string UniqueMarketData = "IX_MarketData_Instrument_Timestamp_TimeFrame";
    }
}



