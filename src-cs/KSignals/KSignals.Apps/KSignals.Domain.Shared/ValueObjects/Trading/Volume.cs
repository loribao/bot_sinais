using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSignals.Domain.Shared.ValueObjects.Trading
{
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
}
