using System.ComponentModel.DataAnnotations;

namespace BotSinais.Domain.Shared;

/// <summary>
/// Classe base para todas as entidades do domínio
/// </summary>
public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public string? CreatedBy { get; set; }
    
    public string? UpdatedBy { get; set; }
    
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Interface para entidades que possuem versionamento
/// </summary>
public interface IVersionedEntity
{
    int Version { get; set; }
}

/// <summary>
/// Interface para entidades que possuem soft delete
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    string? DeletedBy { get; set; }
}

