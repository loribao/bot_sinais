# Exemplos de Implementação - Arquitetura C# + Python

## 1. Backend C# - Controlador de Sinais

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BotSinais.Core.Interfaces;
using BotSinais.Core.Events;

[ApiController]
[Route("api/[controller]")]
public class SignalsController : ControllerBase
{
    private readonly ISignalService _signalService;
    private readonly IHubContext<SignalHub> _hubContext;
    private readonly ILogger<SignalsController> _logger;

    public SignalsController(
        ISignalService signalService,
        IHubContext<SignalHub> hubContext,
        ILogger<SignalsController> logger)
    {
        _signalService = signalService;
        _hubContext = hubContext;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SignalDto>>> GetActiveSignals()
    {
        var signals = await _signalService.GetActiveSignalsAsync();
        return Ok(signals);
    }

    [HttpPost("request-analysis")]
    public async Task<ActionResult> RequestAnalysis([FromBody] AnalysisRequestDto request)
    {
        var correlationId = Guid.NewGuid().ToString();
        
        var marketDataEvent = new MarketDataRequested
        {
            CorrelationId = correlationId,
            Asset = request.Asset,
            Timeframe = request.Timeframe,
            Timestamp = DateTime.UtcNow
        };

        await _signalService.PublishEventAsync(marketDataEvent);
        
        return Accepted(new { CorrelationId = correlationId });
    }

    [HttpGet("performance")]
    public async Task<ActionResult<PerformanceDto>> GetPerformance([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var performance = await _signalService.GetPerformanceAsync(from, to);
        return Ok(performance);
    }
}
```

## 2. C# - Serviço de Sinais com RabbitMQ

```csharp
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using BotSinais.Core.Events;

public interface ISignalService
{
    Task<IEnumerable<SignalDto>> GetActiveSignalsAsync();
    Task PublishEventAsync<T>(T eventData) where T : BaseEvent;
    Task<PerformanceDto> GetPerformanceAsync(DateTime? from, DateTime? to);
}

public class SignalService : ISignalService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ISignalRepository _repository;
    private readonly IHubContext<SignalHub> _hubContext;
    private readonly ILogger<SignalService> _logger;

    public SignalService(
        IConnectionFactory connectionFactory,
        ISignalRepository repository,
        IHubContext<SignalHub> hubContext,
        ILogger<SignalService> logger)
    {
        _repository = repository;
        _hubContext = hubContext;
        _logger = logger;
        
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        
        SetupQueues();
        StartConsuming();
    }

    private void SetupQueues()
    {
        // Queue para enviar comandos ao Python
        _channel.QueueDeclare(
            queue: "python_commands",
            durable: true,
            exclusive: false,
            autoDelete: false);

        // Queue para receber eventos do Python
        _channel.QueueDeclare(
            queue: "csharp_events",
            durable: true,
            exclusive: false,
            autoDelete: false);
    }

    private void StartConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            try
            {
                await ProcessPythonEvent(message);
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar evento do Python: {Message}", message);
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(
            queue: "csharp_events",
            autoAck: false,
            consumer: consumer);
    }

    private async Task ProcessPythonEvent(string message)
    {
        var eventWrapper = JsonSerializer.Deserialize<EventWrapper>(message);
        
        switch (eventWrapper.EventType)
        {
            case "SignalGenerated":
                var signalEvent = JsonSerializer.Deserialize<SignalGenerated>(eventWrapper.Data.ToString());
                await HandleSignalGenerated(signalEvent);
                break;
                
            case "AnalysisCompleted":
                var analysisEvent = JsonSerializer.Deserialize<AnalysisCompleted>(eventWrapper.Data.ToString());
                await HandleAnalysisCompleted(analysisEvent);
                break;
                
            case "HealthCheck":
                _logger.LogInformation("Python engine está ativo");
                break;
                
            default:
                _logger.LogWarning("Evento desconhecido: {EventType}", eventWrapper.EventType);
                break;
        }
    }

    private async Task HandleSignalGenerated(SignalGenerated signalEvent)
    {
        var signal = new Signal
        {
            Id = Guid.NewGuid(),
            Asset = signalEvent.Asset,
            Direction = signalEvent.Direction,
            Confidence = signalEvent.Confidence,
            EntryPrice = signalEvent.EntryPrice,
            StopLoss = signalEvent.StopLoss,
            TakeProfit = signalEvent.TakeProfit,
            Strategy = signalEvent.Strategy,
            Timeframe = signalEvent.Timeframe,
            CreatedAt = signalEvent.Timestamp,
            Status = SignalStatus.Active,
            Indicators = JsonSerializer.Serialize(signalEvent.Indicators)
        };

        await _repository.AddSignalAsync(signal);
        
        // Notifica clientes em tempo real via SignalR
        await _hubContext.Clients.All.SendAsync("NewSignal", signal.ToDto());
        
        _logger.LogInformation("Novo sinal: {Direction} em {Asset} com {Confidence}% de confiança", 
            signal.Direction, signal.Asset, (signal.Confidence * 100).ToString("F1"));
    }

    public async Task PublishEventAsync<T>(T eventData) where T : BaseEvent
    {
        var eventWrapper = new EventWrapper
        {
            EventType = typeof(T).Name,
            Timestamp = DateTime.UtcNow,
            CorrelationId = eventData.CorrelationId,
            Data = eventData
        };

        var message = JsonSerializer.Serialize(eventWrapper);
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: "",
            routingKey: "python_commands",
            basicProperties: null,
            body: body);
    }

    public async Task<IEnumerable<SignalDto>> GetActiveSignalsAsync()
    {
        var signals = await _repository.GetActiveSignalsAsync();
        return signals.Select(s => s.ToDto());
    }

    public async Task<PerformanceDto> GetPerformanceAsync(DateTime? from, DateTime? to)
    {
        var fromDate = from ?? DateTime.UtcNow.AddDays(-30);
        var toDate = to ?? DateTime.UtcNow;
        
        var signals = await _repository.GetSignalsByPeriodAsync(fromDate, toDate);
        
        var totalSignals = signals.Count();
        var winningSignals = signals.Count(s => s.Result == SignalResult.Win);
        var losingSignals = signals.Count(s => s.Result == SignalResult.Loss);
        
        var winRate = totalSignals > 0 ? (double)winningSignals / totalSignals : 0;
        var totalPnL = signals.Sum(s => s.PnL ?? 0);
        
        return new PerformanceDto
        {
            TotalSignals = totalSignals,
            WinningSignals = winningSignals,
            LosingSignals = losingSignals,
            WinRate = winRate,
            TotalPnL = totalPnL,
            Period = new PeriodDto { From = fromDate, To = toDate }
        };
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
```

## 3. C# - SignalR Hub para Real-time

```csharp
using Microsoft.AspNetCore.SignalR;

public class SignalHub : Hub
{
    private readonly ILogger<SignalHub> _logger;

    public SignalHub(ILogger<SignalHub> logger)
    {
        _logger = logger;
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Cliente {ConnectionId} entrou no grupo {GroupName}", 
            Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Cliente {ConnectionId} saiu do grupo {GroupName}", 
            Context.ConnectionId, groupName);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Cliente conectado: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Cliente desconectado: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
```

## 4. Python - Consumer de Eventos RabbitMQ

```python
import asyncio
import json
import logging
from datetime import datetime
from typing import Dict, Any
import aio_pika
from aio_pika.abc import AbstractIncomingMessage

class RabbitMQEventConsumer:
    """Consumer de eventos RabbitMQ para comunicação com C#"""
    
    def __init__(self, connection_url: str, signal_generator):
        self.connection_url = connection_url
        self.signal_generator = signal_generator
        self.connection = None
        self.channel = None
        self.logger = logging.getLogger(__name__)
        
    async def connect(self):
        """Conecta ao RabbitMQ"""
        self.connection = await aio_pika.connect_robust(self.connection_url)
        self.channel = await self.connection.channel()
        
        # Declara filas
        self.commands_queue = await self.channel.declare_queue(
            "python_commands", durable=True
        )
        self.events_queue = await self.channel.declare_queue(
            "csharp_events", durable=True
        )
        
        self.logger.info("Conectado ao RabbitMQ")
    
    async def start_consuming(self):
        """Inicia o consumo de mensagens"""
        await self.commands_queue.consume(self.process_command)
        self.logger.info("Iniciando consumo de comandos do C#")
        
        # Envia heartbeat a cada 30 segundos
        asyncio.create_task(self.send_heartbeat())
    
    async def process_command(self, message: AbstractIncomingMessage):
        """Processa comandos vindos do C#"""
        async with message.process():
            try:
                event_data = json.loads(message.body.decode())
                event_type = event_data.get('eventType')
                correlation_id = event_data.get('correlationId')
                data = event_data.get('data', {})
                
                self.logger.info(f"Processando comando: {event_type}")
                
                if event_type == "MarketDataRequested":
                    await self.handle_market_data_request(data, correlation_id)
                elif event_type == "StrategyConfigurationChanged":
                    await self.handle_strategy_config_change(data, correlation_id)
                elif event_type == "BacktestRequested":
                    await self.handle_backtest_request(data, correlation_id)
                elif event_type == "ModelRetrainingRequested":
                    await self.handle_model_retraining(data, correlation_id)
                else:
                    self.logger.warning(f"Comando desconhecido: {event_type}")
                    
            except Exception as e:
                self.logger.error(f"Erro ao processar comando: {e}")
    
    async def handle_market_data_request(self, data: Dict, correlation_id: str):
        """Processa solicitação de análise de dados de mercado"""
        try:
            asset = data.get('asset')
            timeframe = data.get('timeframe', '5m')
            
            # Coleta dados e gera sinais
            market_data = await self.signal_generator.collect_market_data(asset, timeframe)
            signals = await self.signal_generator.analyze_and_generate_signals(market_data)
            
            # Envia sinais gerados
            for signal in signals:
                await self.publish_signal_generated(signal, correlation_id)
            
            # Notifica conclusão da análise
            await self.publish_analysis_completed(asset, len(signals), correlation_id)
            
        except Exception as e:
            self.logger.error(f"Erro na análise de {data.get('asset')}: {e}")
    
    async def publish_signal_generated(self, signal: Dict, correlation_id: str):
        """Publica evento de sinal gerado"""
        event = {
            "eventType": "SignalGenerated",
            "timestamp": datetime.utcnow().isoformat(),
            "correlationId": correlation_id,
            "data": {
                "asset": signal['asset'],
                "direction": signal['direction'],
                "confidence": signal['confidence'],
                "entryPrice": signal['entry_price'],
                "stopLoss": signal['stop_loss'],
                "takeProfit": signal['take_profit'],
                "strategy": signal['strategy'],
                "timeframe": signal['timeframe'],
                "indicators": signal['indicators']
            }
        }
        
        await self.publish_event(event)
        self.logger.info(f"Sinal publicado: {signal['direction']} em {signal['asset']}")
    
    async def publish_analysis_completed(self, asset: str, signals_count: int, correlation_id: str):
        """Publica evento de análise concluída"""
        event = {
            "eventType": "AnalysisCompleted",
            "timestamp": datetime.utcnow().isoformat(),
            "correlationId": correlation_id,
            "data": {
                "asset": asset,
                "signalsGenerated": signals_count,
                "analysisTime": datetime.utcnow().isoformat()
            }
        }
        
        await self.publish_event(event)
    
    async def publish_event(self, event: Dict):
        """Publica evento para o C#"""
        message_body = json.dumps(event).encode()
        
        await self.channel.default_exchange.publish(
            aio_pika.Message(message_body),
            routing_key="csharp_events"
        )
    
    async def send_heartbeat(self):
        """Envia heartbeat periódico"""
        while True:
            try:
                event = {
                    "eventType": "HealthCheck",
                    "timestamp": datetime.utcnow().isoformat(),
                    "correlationId": None,
                    "data": {
                        "status": "healthy",
                        "uptime": datetime.utcnow().isoformat()
                    }
                }
                
                await self.publish_event(event)
                await asyncio.sleep(30)  # Heartbeat a cada 30 segundos
                
            except Exception as e:
                self.logger.error(f"Erro ao enviar heartbeat: {e}")
                await asyncio.sleep(30)
    
    async def close(self):
        """Fecha conexão"""
        if self.connection:
            await self.connection.close()
```

## 5. Python - Engine Principal de Estratégias

```python
import asyncio
import logging
from datetime import datetime
from typing import Dict, List
from strategies.mean_reversion import MeanReversionStrategy
from strategies.breakout import BreakoutStrategy
from data.collectors import DataCollector
from events.rabbitmq_consumer import RabbitMQEventConsumer

class StrategyEngine:
    """Engine principal de estratégias Python"""
    
    def __init__(self, config: Dict):
        self.config = config
        self.strategies = []
        self.data_collector = DataCollector()
        self.rabbitmq_consumer = None
        self.logger = logging.getLogger(__name__)
        
        # Inicializa estratégias
        self.strategies.append(MeanReversionStrategy())
        self.strategies.append(BreakoutStrategy())
        
    async def start(self):
        """Inicia o engine"""
        self.logger.info("Iniciando Strategy Engine")
        
        # Conecta ao RabbitMQ
        self.rabbitmq_consumer = RabbitMQEventConsumer(
            self.config['rabbitmq_url'], 
            self
        )
        await self.rabbitmq_consumer.connect()
        await self.rabbitmq_consumer.start_consuming()
        
        self.logger.info("Strategy Engine iniciado com sucesso")
        
        # Mantém o processo rodando
        while True:
            await asyncio.sleep(1)
    
    async def collect_market_data(self, asset: str, timeframe: str) -> Dict:
        """Coleta dados de mercado para análise"""
        try:
            async with self.data_collector as collector:
                if asset.endswith('USDT'):
                    # Criptomoeda via Binance
                    data = await collector.get_binance_data(asset, timeframe)
                else:
                    # Ações via Alpha Vantage
                    data = await collector.get_alpha_vantage_data(
                        asset, 
                        self.config['alpha_vantage_key']
                    )
                
                return {asset: data}
                
        except Exception as e:
            self.logger.error(f"Erro ao coletar dados para {asset}: {e}")
            return {}
    
    async def analyze_and_generate_signals(self, market_data: Dict) -> List[Dict]:
        """Analisa dados e gera sinais usando todas as estratégias"""
        signals = []
        
        for asset, data in market_data.items():
            if data.empty:
                continue
                
            for strategy in self.strategies:
                try:
                    signal = strategy.generate_signal(data)
                    if signal:
                        signal['asset'] = asset
                        signals.append(signal)
                        
                except Exception as e:
                    self.logger.error(f"Erro na estratégia {strategy.name} para {asset}: {e}")
        
        return signals
    
    async def stop(self):
        """Para o engine"""
        if self.rabbitmq_consumer:
            await self.rabbitmq_consumer.close()
        self.logger.info("Strategy Engine parado")

# Configuração e execução
async def main():
    config = {
        'rabbitmq_url': 'amqp://guest:guest@localhost:5672/',
        'alpha_vantage_key': 'YOUR_API_KEY'
    }
    
    engine = StrategyEngine(config)
    
    try:
        await engine.start()
    except KeyboardInterrupt:
        await engine.stop()

if __name__ == "__main__":
    logging.basicConfig(level=logging.INFO)
    asyncio.run(main())
```

## 6. Docker Compose - Infraestrutura Completa

```yaml
version: '3.8'

services:
  # RabbitMQ Message Broker
  rabbitmq:
    image: rabbitmq:3-management
    container_name: bot_rabbitmq
    ports:
      - "5672:5672"      # AMQP port
      - "15672:15672"    # Management UI
    environment:
      RABBITMQ_DEFAULT_USER: admin
      RABBITMQ_DEFAULT_PASS: password
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq
    networks:
      - bot_network

  # PostgreSQL Database
  postgres:
    image: postgres:15
    container_name: bot_postgres
    ports:
      - "5432:5432"
    environment:
      POSTGRES_DB: bot_sinais
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: password
    volumes:
      - postgres_data:/var/lib/postgresql/data
    networks:
      - bot_network

  # Redis Cache
  redis:
    image: redis:7-alpine
    container_name: bot_redis
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    networks:
      - bot_network

  # Backend C# API
  api_csharp:
    build:
      context: ./backend_csharp
      dockerfile: Dockerfile
    container_name: bot_api_csharp
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      ConnectionStrings__DefaultConnection: "Server=postgres;Database=bot_sinais;User Id=postgres;Password=password;"
      ConnectionStrings__Redis: "redis:6379"
      RabbitMQ__ConnectionString: "amqp://admin:password@rabbitmq:5672/"
    depends_on:
      - postgres
      - redis
      - rabbitmq
    networks:
      - bot_network

  # Python Strategy Engine
  strategy_engine:
    build:
      context: ./python_strategies
      dockerfile: Dockerfile
    container_name: bot_strategy_engine
    environment:
      RABBITMQ_URL: "amqp://admin:password@rabbitmq:5672/"
      POSTGRES_URL: "postgresql://postgres:password@postgres:5432/bot_sinais"
      REDIS_URL: "redis://redis:6379"
    depends_on:
      - rabbitmq
      - postgres
      - redis
    networks:
      - bot_network

volumes:
  rabbitmq_data:
  postgres_data:
  redis_data:

networks:
  bot_network:
    driver: bridge
```

Esta arquitetura fornece uma separação clara entre o backend web (C#) e o engine de estratégias (Python), com comunicação assíncrona via RabbitMQ, permitindo escalabilidade e manutenibilidade superiores.
