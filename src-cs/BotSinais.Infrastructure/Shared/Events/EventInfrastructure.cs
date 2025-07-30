using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Logging;
using BotSinais.Domain.Shared.Events;

namespace BotSinais.Infrastructure.Shared.Events;

/// <summary>
/// Interface para publicação de eventos de domínio
/// </summary>
public interface IDomainEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IDomainEvent;
    Task PublishBatchAsync<T>(IEnumerable<T> events, CancellationToken cancellationToken = default) where T : class, IDomainEvent;
}

/// <summary>
/// Implementação do publicador de eventos usando MassTransit
/// </summary>
public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public DomainEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IDomainEvent
    {
        await _publishEndpoint.Publish(@event, cancellationToken);
    }

    public async Task PublishBatchAsync<T>(IEnumerable<T> events, CancellationToken cancellationToken = default) where T : class, IDomainEvent
    {
        var tasks = events.Select(e => _publishEndpoint.Publish(e, cancellationToken));
        await Task.WhenAll(tasks);
    }
}

/// <summary>
/// Interface base para consumidores de eventos
/// </summary>
public interface IDomainEventHandler<in T> : IConsumer<T> where T : class, IDomainEvent
{
}

/// <summary>
/// Classe base para manipuladores de eventos de domínio
/// </summary>
public abstract class DomainEventHandler<T> : IDomainEventHandler<T> where T : class, IDomainEvent
{
    protected readonly ILogger<DomainEventHandler<T>> Logger;

    protected DomainEventHandler(ILogger<DomainEventHandler<T>> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<T> context)
    {
        try
        {
            Logger.LogInformation("Processing event {EventType} with ID {EventId}", 
                context.Message.EventType, context.Message.EventId);
            
            await HandleAsync(context.Message, context.CancellationToken);
            
            Logger.LogInformation("Successfully processed event {EventType} with ID {EventId}", 
                context.Message.EventType, context.Message.EventId);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error processing event {EventType} with ID {EventId}: {Error}", 
                context.Message.EventType, context.Message.EventId, ex.Message);
            throw;
        }
    }

    protected abstract Task HandleAsync(T @event, CancellationToken cancellationToken);
}

/// <summary>
/// Configurações para o MassTransit
/// </summary>
public static class MassTransitConfiguration
{
    /// <summary>
    /// Configura o MassTransit com InMemory para desenvolvimento
    /// </summary>
    public static void ConfigureDevelopmentBus(IBusRegistrationConfigurator configurator)
    {
        configurator.UsingInMemory((context, cfg) =>
        {
            // Configuração de retry
            cfg.UseMessageRetry(r => r.Exponential(3, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(5)));
            
            cfg.ConfigureEndpoints(context);
        });
    }
    
    /// <summary>
    /// Configura o MassTransit com RabbitMQ para produção
    /// </summary>
    public static void ConfigureProductionBus(IBusRegistrationConfigurator configurator, string connectionString)
    {
        configurator.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(connectionString);
            
            // Configuração de retry
            cfg.UseMessageRetry(r => r.Exponential(3, TimeSpan.FromSeconds(1), TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(5)));
            
            // Configuração de circuito
            cfg.UseCircuitBreaker(cb =>
            {
                cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                cb.TripThreshold = 15;
                cb.ActiveThreshold = 10;
                cb.ResetInterval = TimeSpan.FromMinutes(5);
            });
            
            // Configurações de endpoints para cada contexto
            ConfigureDataManagementEndpoints(cfg, context);
            ConfigureSignalsEndpoints(cfg, context);
            ConfigureStrategiesEndpoints(cfg, context);
            
            cfg.ConfigureEndpoints(context);
        });
    }

    /// <summary>
    /// Configura o bus baseado no ambiente (Development/Production)
    /// </summary>
    public static void ConfigureBus(IBusRegistrationConfigurator configurator, bool isDevelopment, string? connectionString = null)
    {
        if (isDevelopment)
        {
            ConfigureDevelopmentBus(configurator);
        }
        else
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string is required for production environment", nameof(connectionString));
            
            ConfigureProductionBus(configurator, connectionString);
        }
    }
    
    private static void ConfigureDataManagementEndpoints(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint("data-management-events", e =>
        {
            e.PrefetchCount = 20;
            e.ConfigureConsumers(context);
        });
    }
    
    private static void ConfigureSignalsEndpoints(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint("signals-events", e =>
        {
            e.PrefetchCount = 50;
            e.ConfigureConsumers(context);
        });
    }
    
    private static void ConfigureStrategiesEndpoints(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint("strategies-events", e =>
        {
            e.PrefetchCount = 10;
            e.ConfigureConsumers(context);
        });
    }
}

/// <summary>
/// Extensões para registro de manipuladores de eventos
/// </summary>
public static class EventHandlerRegistrationExtensions
{
    /// <summary>
    /// Adiciona o MassTransit configurado para o ambiente
    /// </summary>
    public static IServiceCollection AddMassTransitWithConfiguration(
        this IServiceCollection services, 
        bool isDevelopment, 
        string? rabbitMqConnectionString = null,
        params Assembly[] assembliesWithHandlers)
    {
        services.AddMassTransit(x =>
        {
            // Registra todos os consumidores dos assemblies fornecidos
            foreach (var assembly in assembliesWithHandlers)
            {
                x.AddConsumers(assembly);
            }

            // Configura o transport baseado no ambiente
            MassTransitConfiguration.ConfigureBus(x, isDevelopment, rabbitMqConnectionString);
        });

        // Registra o publisher customizado
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();

        return services;
    }

    public static IServiceCollection AddDomainEventHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetInterfaces().Any(i => 
                i.IsGenericType && 
                i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            var eventTypes = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                .Select(i => i.GetGenericArguments()[0])
                .ToList();

            foreach (var eventType in eventTypes)
            {
                var interfaceType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
                services.AddScoped(interfaceType, handlerType);
            }
        }

        return services;
    }
}

