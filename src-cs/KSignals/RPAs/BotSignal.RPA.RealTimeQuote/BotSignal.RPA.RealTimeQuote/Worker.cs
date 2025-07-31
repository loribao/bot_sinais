using MassTransit;
using MassTransit.Internals.GraphValidation;
using MassTransit.RabbitMqTransport.Topology;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace BotSignal.RPA.RealTimeQuote;

public class Worker : BackgroundService
{
    readonly IBus _bus;

    public Worker(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var host = Environment.GetEnvironmentVariable("ConnectionStrings__messaging");

        var endpoint = await _bus.GetSendEndpoint(new Uri(new Uri(host), relativeUri: "trading-data-events"));
        while (!stoppingToken.IsCancellationRequested)
        {
            await endpoint.Send(new
            {
                Close = 100.0m,
                EventId = Guid.NewGuid(),
                High = 101.0m,
                InstrumentId = Guid.NewGuid(),
                Low = 99.0m,
                Open = 100.5m,
                Timestamp = DateTime.UtcNow,
                Volume = 10000,
                OccurredAt = DateTime.UtcNow,
                Symbol = "BTCUSD@Binance",
                TimeFrame = "M1",
            }, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
