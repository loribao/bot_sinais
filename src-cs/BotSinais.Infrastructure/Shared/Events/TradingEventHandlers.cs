using BotSinais.Domain.Shared.Events;
using Microsoft.Extensions.Logging;
using InfrastructureEventPublisher = BotSinais.Infrastructure.Shared.Events.IDomainEventPublisher;

// Referências diretas aos eventos organizados
using MarketDataReceivedEvent = BotSinais.Domain.Shared.Events.DataManagement.MarketDataReceivedEvent;
using SignalGeneratedEvent = BotSinais.Domain.Shared.Events.Signals.SignalGeneratedEvent;
using SystemErrorEvent = BotSinais.Domain.Shared.Events.System.SystemErrorEvent;

namespace BotSinais.Infrastructure.Shared.Events;

/// <summary>
/// Handler para eventos de dados de mercado recebidos
/// </summary>
public class MarketDataReceivedHandler : DomainEventHandler<MarketDataReceivedEvent>
{
    private readonly InfrastructureEventPublisher _eventPublisher;

    public MarketDataReceivedHandler(
        ILogger<MarketDataReceivedHandler> logger,
        InfrastructureEventPublisher eventPublisher) : base(logger)
    {
        _eventPublisher = eventPublisher;
    }

    protected override async Task HandleAsync(MarketDataReceivedEvent @event, CancellationToken cancellationToken)
    {
        Logger.LogInformation(
            "Processando dados de mercado: {Symbol} - Fechamento: {Close} - Volume: {Volume}",
            @event.Symbol.Code,
            @event.Close.Value,
            @event.Volume.Value);

        // Aqui você implementaria a lógica de processamento dos dados de mercado
        // Por exemplo: armazenar no banco, executar análises técnicas, etc.

        // Exemplo: Se o preço de fechamento subiu muito, gerar um evento de alerta
        if (@event.Close.Value > 100) // Exemplo simples
        {
            var alertEvent = new SystemErrorEvent
            {
                Component = "MarketDataProcessor",
                ErrorType = "PriceAlert",
                Message = $"Preço alto detectado para {@event.Symbol.Code}: {@event.Close.Value}",
                Context = new Dictionary<string, object>
                {
                    ["InstrumentId"] = @event.InstrumentId,
                    ["Symbol"] = @event.Symbol.Code,
                    ["Close"] = @event.Close.Value,
                    ["Threshold"] = 100
                }
            };

            await _eventPublisher.PublishAsync(alertEvent, cancellationToken);
        }

        await Task.Delay(100, cancellationToken); // Simula processamento
    }
}

/// <summary>
/// Handler para eventos de sinais gerados
/// </summary>
public class SignalGeneratedHandler : DomainEventHandler<SignalGeneratedEvent>
{
    private readonly InfrastructureEventPublisher _eventPublisher;

    public SignalGeneratedHandler(
        ILogger<SignalGeneratedHandler> logger,
        InfrastructureEventPublisher eventPublisher) : base(logger)
    {
        _eventPublisher = eventPublisher;
    }

    protected override async Task HandleAsync(SignalGeneratedEvent @event, CancellationToken cancellationToken)
    {
        Logger.LogInformation(
            "Processando sinal gerado: {SignalId} - {Direction} - Confiança: {Confidence}%",
            @event.SignalId,
            @event.Direction,
            @event.Confidence);

        // Aqui você implementaria a lógica de processamento do sinal
        // Por exemplo: validação, armazenamento, execução automática, etc.

        // Exemplo: Se a confiança for alta, log especial
        if (@event.Confidence >= 80)
        {
            Logger.LogInformation(
                "Sinal de alta confiança detectado: {SignalId} com {Confidence}% de confiança",
                @event.SignalId,
                @event.Confidence);
        }

        await Task.Delay(50, cancellationToken); // Simula processamento
    }
}

/// <summary>
/// Handler para eventos de erro do sistema
/// </summary>
public class SystemErrorHandler : DomainEventHandler<SystemErrorEvent>
{
    public SystemErrorHandler(ILogger<SystemErrorHandler> logger) : base(logger) { }

    protected override async Task HandleAsync(SystemErrorEvent @event, CancellationToken cancellationToken)
    {
        Logger.LogError(
            "Erro no sistema detectado - Componente: {Component}, Tipo: {ErrorType}, Mensagem: {Message}",
            @event.Component,
            @event.ErrorType,
            @event.Message);

        // Aqui você implementaria a lógica de tratamento de erros
        // Por exemplo: notificações, alertas, logging especializado, etc.

        await Task.Delay(10, cancellationToken); // Simula processamento
    }
}
