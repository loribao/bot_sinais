using KSignals.Domain.Shared.Entities;
using KSignals.Domain.Shared.ValueObjects.Person;

namespace Ksignals.Apps.Auth.Core.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; } = 0;
        public FullName Name { get; set; }
        public Email Email { get; set; }
        public Password Passs { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
