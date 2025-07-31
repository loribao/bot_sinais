using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSignals.Domain.Shared.ValueObjects.Person;

public record FullName
{
    public string FirstName { get; }
    public IReadOnlyList<string> MiddleNames { get; }
    public string LastName { get; }

    public FullName(string firstName, IEnumerable<string> middleNames, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Primeiro nome não pode ser vazio.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Último nome não pode ser vazio.", nameof(lastName));

        FirstName = firstName.Trim();
        MiddleNames = middleNames?.Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.Trim()).ToArray() ?? Array.Empty<string>();
        LastName = lastName.Trim();
    }

    public FullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Nome completo não pode ser vazio.", nameof(fullName));

        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
            throw new ArgumentException("Nome completo deve conter pelo menos nome e sobrenome.", nameof(fullName));

        FirstName = parts.First();
        LastName = parts.Last();
        MiddleNames = parts.Skip(1).Take(parts.Length - 2).ToArray();
    }

    public override string ToString()
    {
        if (MiddleNames.Count == 0)
            return $"{FirstName} {LastName}";
        return $"{FirstName} {string.Join(" ", MiddleNames)} {LastName}";
    }
}
