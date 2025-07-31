using System.Text.RegularExpressions;

namespace KSignals.Domain.Shared.ValueObjects.Person
{
    public record Email
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; init; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O e-mail não pode ser vazio.", nameof(value));

            if (!EmailRegex.IsMatch(value))
                throw new ArgumentException("Formato de e-mail inválido.", nameof(value));

            Value = value;
        }

        public override string ToString() => Value;

        // Records automatically provide value-based equality, but we customize for case-insensitive comparison
        public virtual bool Equals(Email? other) =>
            other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
    }
}