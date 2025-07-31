using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSignals.Domain.Shared.ValueObjects.Trading
{
    public record Price
    {
        public decimal Value { get; init; }
        public string Currency { get; init; } = "USD";

        public Price(decimal value, string currency = "USD")
        {
            if (value < 0)
                throw new ArgumentException("Price cannot be negative", nameof(value));

            Value = value;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }

        public static implicit operator decimal(Price price) => price.Value;
        public static implicit operator Price(decimal value) => new(value);

        public override string ToString() => $"{Value:F2} {Currency}";
    }
}
