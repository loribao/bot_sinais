namespace BotSinais.Domain.Shared.Events;

/// <summary>
/// Interface para publicação de eventos de domínio
/// Abstrai a implementação específica de message bus
/// </summary>
public interface IDomainEventPublisher
{
    /// <summary>
    /// Publica um evento de domínio de forma assíncrona
    /// </summary>
    /// <typeparam name="T">Tipo do evento</typeparam>
    /// <param name="domainEvent">Evento a ser publicado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;

    /// <summary>
    /// Publica múltiplos eventos de domínio de forma assíncrona
    /// </summary>
    /// <param name="domainEvents">Eventos a serem publicados</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    Task PublishAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
