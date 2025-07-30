namespace BotSinais.Domain.Shared.Interfaces;

/// <summary>
/// Interface base para queries CQRS
/// </summary>
public interface IQuery
{
    /// <summary>
    /// Identificador único da query
    /// </summary>
    Guid QueryId { get; }
    
    /// <summary>
    /// Data/hora de criação da query
    /// </summary>
    DateTime CreatedAt { get; }
    
    /// <summary>
    /// Usuário que executou a query
    /// </summary>
    string? UserId { get; }
}

/// <summary>
/// Interface para queries que retornam resultado
/// </summary>
/// <typeparam name="TResult">Tipo do resultado</typeparam>
public interface IQuery<TResult> : IQuery
{
}

/// <summary>
/// Classe base abstrata para implementação de queries
/// </summary>
public abstract record BaseQuery : IQuery
{
    public Guid QueryId { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string? UserId { get; init; }
}

/// <summary>
/// Classe base abstrata para queries com resultado
/// </summary>
/// <typeparam name="TResult">Tipo do resultado</typeparam>
public abstract record BaseQuery<TResult> : BaseQuery, IQuery<TResult>
{
}

/// <summary>
/// Interface para handlers de query
/// </summary>
/// <typeparam name="TQuery">Tipo da query</typeparam>
/// <typeparam name="TResult">Tipo do resultado</typeparam>
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}

