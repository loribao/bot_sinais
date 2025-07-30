# Prompt para Construção de Bot de Sinais de Trading

## Visão Geral
Desenvolver um bot inteligente de sinais de trading que analise múltiplos mercados financeiros (opções binárias, criptomoedas, ações, FIIs e stocks) e forneça sinais de entrada precisos com base em análise técnica avançada e machine learning.

## Especificações Técnicas

### 1. Arquitetura do Sistema
- **Backend Web**: C# .NET 8+ com ASP.NET Core
- **Engine de Estratégias**: Python 3.9+
- **Communication**: RabbitMQ para eventos entre C# e Python
- **Banco de Dados**: PostgreSQL para dados históricos, Redis para cache
- **Frontend**: Blazor Server/WASM ou React
- **Containerização**: Docker + Docker Compose
- **Monitoramento**: Prometheus + Grafana

### 2. Mercados Suportados
- **Opções Binárias**: EUR/USD, USD/JPY, GBP/USD, AUD/USD
- **Criptomoedas**: BTC, ETH, BNB, ADA, SOL, MATIC, DOT
- **Ações Brasileiras**: PETR4, VALE3, ITUB4, BBDC4, ABEV3
- **FIIs**: HGLG11, KNRI11, XPML11, BTLG11
- **Stocks Americanas**: AAPL, GOOGL, MSFT, TSLA, NVDA

### 3. Indicadores Técnicos Implementados

#### Indicadores de Tendência
- **Médias Móveis**: SMA, EMA, WMA
- **MACD**: Signal Line, Histogram
- **ADX**: Directional Movement Index
- **Parabolic SAR**
- **Ichimoku Cloud**

#### Indicadores de Momentum
- **RSI**: Relative Strength Index
- **Stochastic**: %K e %D
- **Williams %R**
- **CCI**: Commodity Channel Index
- **MFI**: Money Flow Index

#### Indicadores de Volatilidade
- **Bollinger Bands**
- **ATR**: Average True Range
- **Keltner Channels**
- **Donchian Channels**

#### Indicadores de Volume
- **OBV**: On-Balance Volume
- **VWAP**: Volume Weighted Average Price
- **Volume Profile**
- **Accumulation/Distribution Line**

### 4. Estratégias de Trading

#### Estratégia 1: Reversão de Médias
```
Condições de CALL:
- Preço abaixo da EMA(20) por mais de 3 períodos
- RSI < 30 (sobrevenda)
- Volume acima da média dos últimos 20 períodos
- MACD histogram mostrando divergência positiva

Condições de PUT:
- Preço acima da EMA(20) por mais de 3 períodos
- RSI > 70 (sobrecompra)
- Volume acima da média dos últimos 20 períodos
- MACD histogram mostrando divergência negativa
```

#### Estratégia 2: Breakout com Volume
```
Condições de CALL:
- Preço rompendo Bollinger Band superior
- Volume 2x maior que a média
- ADX > 25 (tendência forte)
- RSI entre 50-70

Condições de PUT:
- Preço rompendo Bollinger Band inferior
- Volume 2x maior que a média
- ADX > 25 (tendência forte)
- RSI entre 30-50
```

#### Estratégia 3: Confluência Multi-Timeframe
```
Análise em 3 timeframes (1m, 5m, 15m):
- Alinhamento de tendência em todos os timeframes
- Confluência de suporte/resistência
- Padrões de candlestick de reversão
- Divergências em indicadores
```

### 5. Sistema de Machine Learning

#### Modelo de Classificação
- **Algoritmo**: Random Forest + XGBoost Ensemble
- **Features**: 50+ indicadores técnicos normalizados
- **Target**: Direção do movimento (CALL/PUT/NEUTRO)
- **Retreinamento**: Diário com dados das últimas 6 semanas

#### Pipeline de Dados
```
Raw Data → Feature Engineering → Normalization → Model Training → Prediction
```

#### Features Principais
- Indicadores técnicos (30 features)
- Padrões de candlestick (10 features)
- Análise de sentimento (5 features)
- Dados macroeconômicos (5 features)
- Correlações entre ativos (5 features)

### 5.1. Comunicação C# ↔ Python via RabbitMQ

#### Eventos de Comunicação
```
C# → Python:
- MarketDataRequested: Solicita análise de ativo
- StrategyConfigurationChanged: Atualiza parâmetros
- BacktestRequested: Solicita backtesting
- ModelRetrainingRequested: Solicita retreinamento ML

Python → C#:
- SignalGenerated: Novo sinal de trading
- AnalysisCompleted: Análise finalizada
- ModelUpdated: Modelo ML atualizado
- HealthCheck: Status do engine Python
```

#### Estrutura dos Eventos
```json
{
  "eventType": "SignalGenerated",
  "timestamp": "2025-07-29T10:30:00Z",
  "correlationId": "uuid-v4",
  "data": {
    "asset": "BTCUSDT",
    "direction": "CALL",
    "confidence": 0.85,
    "entryPrice": 45250.00,
    "stopLoss": 44950.00,
    "takeProfit": 45850.00,
    "strategy": "MeanReversion",
    "timeframe": "5m",
    "indicators": {
      "rsi": 28.5,
      "bbPosition": "below_lower_band"
    }
  }
}
```

### 6. Gestão de Risco

#### Risk Management Rules
- **Stop Loss**: 2% do capital por operação
- **Take Profit**: 1:2 risk/reward mínimo
- **Máximo de operações simultâneas**: 5
- **Drawdown máximo**: 10%
- **Filtro de notícias**: Evitar trading durante eventos high-impact

#### Sizing Algorithm
```python
def calculate_position_size(capital, risk_percent, stop_loss_pips):
    risk_amount = capital * (risk_percent / 100)
    position_size = risk_amount / stop_loss_pips
    return min(position_size, capital * 0.05)  # Max 5% do capital
```

### 7. Fontes de Dados

#### APIs de Preços
- **Alpha Vantage**: Ações e FIIs
- **Binance API**: Criptomoedas
- **IEX Cloud**: Stocks americanas
- **MetaTrader 5**: Forex e opções binárias

#### Dados Fundamentalistas
- **Yahoo Finance**: Dados corporativos
- **Federal Reserve Economic Data (FRED)**: Dados macroeconômicos
- **CVM**: Dados de FIIs brasileiros

### 8. Sistema de Notificações

#### Canais de Comunicação
- **Telegram Bot**: Sinais instantâneos
- **Discord Webhook**: Alertas para comunidade
- **Email**: Relatórios diários/semanais
- **Push Notifications**: App mobile

#### Formato do Sinal
```
🔥 SINAL DE ENTRADA 🔥

💰 Ativo: BTC/USDT
📈 Direção: CALL
⏰ Timeframe: 5 minutos
🎯 Entry: $45,250
🛑 Stop Loss: $44,950
💎 Take Profit: $45,850
📊 Confiança: 87%
🤖 Estratégia: Breakout + ML

📈 Indicadores:
• RSI: 45.2
• MACD: Bullish Cross
• Volume: 142% da média
• Suporte: $45,200
```

### 9. Backtesting e Otimização

#### Métricas de Performance
- **Win Rate**: % de operações vencedoras
- **Profit Factor**: Lucro bruto / Perda bruta
- **Sharpe Ratio**: Retorno ajustado ao risco
- **Maximum Drawdown**: Maior perda consecutiva
- **Average Trade**: Resultado médio por operação

#### Período de Teste
- **Dados históricos**: 2 anos
- **Walk-forward analysis**: 6 meses
- **Monte Carlo simulation**: 1000 cenários

### 10. Interface de Usuário

#### Dashboard Web
- **Real-time charts** com TradingView
- **Lista de sinais ativos**
- **Performance metrics**
- **Configurações de estratégia**
- **Histórico de operações**

#### Mobile App (React Native)
- **Push notifications**
- **Sinais simplificados**
- **Quick actions**
- **Performance summary**

### 11. Segurança e Compliance

#### Medidas de Segurança
- **API Rate Limiting**
- **JWT Authentication**
- **Encrypted database**
- **2FA para usuários**
- **Audit logs**

#### Disclaimers Legais
- **Não é consultoria financeira**
- **Riscos inerentes ao trading**
- **Resultados passados não garantem futuros**
- **Uso por conta e risco**

### 12. Estrutura de Código Sugerida

```
bot_sinais/
├── backend_csharp/                    # Backend Web em C#
│   ├── BotSinais.Api/                # API REST
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Services/
│   │   └── Program.cs
│   ├── BotSinais.Core/               # Lógica de negócio
│   │   ├── Entities/
│   │   ├── Interfaces/
│   │   ├── Services/
│   │   └── Events/
│   ├── BotSinais.Infrastructure/     # Infraestrutura
│   │   ├── Data/
│   │   ├── Messaging/
│   │   ├── External/
│   │   └── Repositories/
│   ├── BotSinais.Web/               # Frontend Web
│   │   ├── Components/
│   │   ├── Pages/
│   │   ├── Services/
│   │   └── wwwroot/
│   └── BotSinais.Tests/             # Testes
├── python_strategies/                # Engine de Estratégias Python
│   ├── src/
│   │   ├── core/
│   │   │   ├── config.py
│   │   │   ├── database.py
│   │   │   └── messaging.py
│   │   ├── data/
│   │   │   ├── collectors/
│   │   │   ├── processors/
│   │   │   └── validators/
│   │   ├── indicators/
│   │   │   ├── trend.py
│   │   │   ├── momentum.py
│   │   │   ├── volatility.py
│   │   │   └── volume.py
│   │   ├── strategies/
│   │   │   ├── base_strategy.py
│   │   │   ├── mean_reversion.py
│   │   │   ├── breakout.py
│   │   │   └── ml_strategy.py
│   │   ├── ml/
│   │   │   ├── models/
│   │   │   ├── features/
│   │   │   └── training/
│   │   ├── risk/
│   │   │   ├── position_sizing.py
│   │   │   ├── risk_manager.py
│   │   │   └── portfolio.py
│   │   └── events/
│   │       ├── handlers/
│   │       ├── publishers/
│   │       └── consumers/
│   ├── tests/
│   ├── requirements.txt
│   └── main.py
├── docker/
│   ├── docker-compose.yml
│   ├── Dockerfile.csharp
│   ├── Dockerfile.python
│   └── rabbitmq/
├── docs/
└── README.md
```

### 13. Fases de Desenvolvimento

#### Fase 1: MVP (4 semanas)
- Setup básico do projeto
- Coleta de dados de 3 ativos
- 5 indicadores principais
- 1 estratégia simples
- Notificações via Telegram

#### Fase 2: Expansão (6 semanas)
- Todos os ativos suportados
- Todos os indicadores técnicos
- 3 estratégias completas
- Dashboard web básico
- Backtesting engine

#### Fase 3: ML e Otimização (8 semanas)
- Modelo de machine learning
- Sistema de risk management
- Dashboard avançado
- Mobile app
- Otimizações de performance

#### Fase 4: Produção (4 semanas)
- Deployment na nuvem
- Monitoramento completo
- Documentação final
- Testes de stress
- Go-live

### 14. Considerações de Performance

#### Otimizações
- **Caching**: Redis para dados frequentes
- **Async processing**: Celery para tarefas pesadas
- **Database indexing**: Otimização de queries
- **CDN**: Para assets estáticos
- **Load balancing**: Para alta disponibilidade

#### Escalabilidade
- **Horizontal scaling**: Múltiplas instâncias
- **Microservices**: Separação por funcionalidade
- **Message queues**: Para processamento distribuído
- **Auto-scaling**: Baseado em CPU/memória

### 15. Monitoramento e Alertas

#### Métricas do Sistema
- **Latência das APIs**
- **Taxa de erro**
- **Uso de CPU/memória**
- **Throughput de sinais**
- **Uptime do sistema**

#### Alertas Críticos
- **API down**: Notificação imediata
- **Perda de conexão com dados**
- **Erro no modelo ML**
- **Drawdown excessivo**
- **Volume anormal de sinais**

Este prompt fornece uma base sólida para desenvolvimento de um bot de sinais profissional e completo. Cada seção pode ser expandida conforme necessário durante o desenvolvimento.
