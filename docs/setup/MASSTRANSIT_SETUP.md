# Configuração MassTransit para o Bot de Sinais

## Visão Geral

O sistema foi configurado com uma arquitetura híbrida que permite uso em desenvolvimento (InMemory) e produção (RabbitMQ) de forma transparente.

## Configuração

### Desenvolvimento (InMemory)
- **Transporte**: InMemory (sem dependências externas)
- **Configuração**: Automática quando `IsDevelopment()` é true
- **Vantagens**: Não requer RabbitMQ instalado, inicialização rápida

### Produção (RabbitMQ)
- **Transporte**: RabbitMQ
- **Configuração**: Via connection string no appsettings.json
- **Connection String**: `"RabbitMQ": "amqp://guest:guest@localhost:5672/"`

## Uso na API

### 1. Registro do Serviço (Program.cs)
```csharp
// Configuração automática baseada no ambiente
builder.Services.AddMassTransitWithConfiguration(
    isDevelopment: builder.Environment.IsDevelopment(),
    rabbitMqConnectionString: builder.Configuration.GetConnectionString("RabbitMQ"),
    assembliesWithHandlers: Assembly.GetExecutingAssembly()
);

// Registro de handlers específicos da API
builder.Services.AddDomainEventHandlers(Assembly.GetExecutingAssembly());
```

### 2. Endpoints da API

#### Receber Dados de Mercado
**POST** `/api/TradingSignals/market-data`
```json
{
  "instrumentId": "123e4567-e89b-12d3-a456-426614174000",
  "symbol": "BTCUSD",
  "exchange": "BINANCE",
  "open": 45000.50,
  "high": 45500.75,
  "low": 44800.25,
  "close": 45200.00,
  "volume": 1500000,
  "timestamp": "2024-01-15T10:30:00Z",
  "timeFrame": "M1"
}
```

#### Gerar Sinal de Trading
**POST** `/api/TradingSignals/generate-signal`
```json
{
  "instrumentId": "123e4567-e89b-12d3-a456-426614174000",
  "strategyId": "987fcdeb-51a2-43d1-b456-426614174999",
  "direction": "Buy",
  "entryPrice": 45200.00,
  "stopLoss": 44500.00,
  "takeProfit": 46000.00,
  "confidence": 85.5
}
```

## Event Handlers

### MarketDataReceivedHandler
- **Evento**: `MarketDataReceivedEvent`
- **Função**: Processa dados de mercado recebidos
- **Comportamento**: 
  - Log dos dados recebidos
  - Alerta automático para preços acima de threshold
  - Processamento assíncrono

### SignalGeneratedHandler
- **Evento**: `SignalGeneratedEvent`
- **Função**: Processa sinais de trading gerados
- **Comportamento**:
  - Log do sinal com nível de confiança
  - Tratamento especial para sinais de alta confiança (≥80%)
  - Simulação de processamento

### SystemErrorHandler
- **Evento**: `SystemErrorEvent`
- **Função**: Trata erros e alertas do sistema
- **Comportamento**:
  - Log estruturado de erros
  - Contexto adicional para debugging

## Estrutura de Eventos

### Eventos de Dados
- `MarketDataReceivedEvent`: Novos dados de mercado
- `MarketDataUpdatedEvent`: Atualizações de dados

### Eventos de Sinais
- `SignalGeneratedEvent`: Novo sinal criado
- `SignalExecutedEvent`: Sinal executado
- `SignalStatusChangedEvent`: Mudança no status do sinal

### Eventos de Sistema
- `SystemErrorEvent`: Erros e alertas
- `StrategyExecutionStartedEvent`: Início de execução de estratégia
- `StrategyExecutionCompletedEvent`: Fim de execução de estratégia

## Debugging e Logs

### Logs Estruturados
```csharp
Logger.LogInformation(
    "Processando dados de mercado: {Symbol} - Fechamento: {Close} - Volume: {Volume}",
    @event.Symbol.Code,
    @event.Close.Value,
    @event.Volume.Value);
```

### Rastreamento de Eventos
- Cada evento tem `EventId` único
- Timestamp automático (`OccurredAt`)
- Tipo de evento identificado (`EventType`)

## Próximos Passos

1. **Repositórios**: Implementar persistência dos dados
2. **Validações**: Adicionar validação de entrada nos controllers
3. **Autenticação**: Implementar autenticação/autorização
4. **Rate Limiting**: Controle de taxa para APIs
5. **Health Checks**: Monitoramento da saúde do sistema
6. **Métricas**: Coleta de métricas de performance
7. **Testes**: Testes unitários e de integração

## Arquitetura Limpa

O sistema segue os princípios da Clean Architecture:
- **Domain**: Puro, sem dependências externas
- **Infrastructure**: Implementações concretas (MassTransit, EventHandlers)
- **ApiService**: Interface HTTP, orquestração

### Dependências
```
ApiService → Infrastructure → Domain
ApiService → Domain
Infrastructure ↛ ApiService (não depende)
Domain ↛ Infrastructure (não depende)
Domain ↛ ApiService (não depende)
```
