namespace BotSinais.Domain.Shared.Interfaces;

/// <summary>
/// Interface base para comandos CQRS
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Identificador único do comando
    /// </summary>
    Guid CommandId { get; }
    
    /// <summary>
    /// Data/hora de criação do comando
    /// </summary>
    DateTime CreatedAt { get; }
    
    /// <summary>
    /// Usuário que executou o comando
    /// </summary>
    string? UserId { get; }
}

/// <summary>
/// Interface para comandos que retornam resultado
/// </summary>
/// <typeparam name="TResult">Tipo do resultado</typeparam>
public interface ICommand<TResult> : ICommand
{
}

/// <summary>
/// Classe base abstrata para implementação de comandos
/// </summary>
public abstract record BaseCommand : ICommand
{
    public Guid CommandId { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string? UserId { get; init; }
}

/// <summary>
/// Classe base abstrata para comandos com resultado
/// </summary>
/// <typeparam name="TResult">Tipo do resultado</typeparam>
public abstract record BaseCommand<TResult> : BaseCommand, ICommand<TResult>
{
}

