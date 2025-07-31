using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace KSignals.Domain.Shared.ValueObjects.Person
{
    public record Password
    {
        private static readonly Regex PasswordRegex = new(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            RegexOptions.Compiled);

        public string Value { get; init; }
        public string Hash { get; init; }

        public Password(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("A senha não pode ser vazia.", nameof(value));

            if (value.Length < 8)
                throw new ArgumentException("A senha deve ter pelo menos 8 caracteres.", nameof(value));

            if (value.Length > 128)
                throw new ArgumentException("A senha não pode ter mais de 128 caracteres.", nameof(value));

            if (!PasswordRegex.IsMatch(value))
                throw new ArgumentException(
                    "A senha deve conter pelo menos: 1 letra minúscula, 1 maiúscula, 1 número e 1 caractere especial (@$!%*?&).",
                    nameof(value));

            Value = value;
            Hash = HashPassword(value);
        }

        private Password(string value, string hash)
        {
            Value = value;
            Hash = hash;
        }

        private static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            var hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            try
            {
                var hashBytes = Convert.FromBase64String(Hash);
                var salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);

                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
                var hash = pbkdf2.GetBytes(32);

                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Password FromHash(string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
                throw new ArgumentException("Hash da senha não pode ser vazio.", nameof(hashedPassword));

            return new Password("********", hashedPassword);
        }

        public override string ToString() => "********";

        // Records automatically provide value-based equality, but we override to use Hash comparison for security
        public virtual bool Equals(Password? other) =>
            other is not null && string.Equals(Hash, other.Hash, StringComparison.Ordinal);

        public override int GetHashCode() => Hash.GetHashCode();
    }
}

