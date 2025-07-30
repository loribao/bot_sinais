# Setup e Configuração do Projeto

## 🎯 Visão Geral da Arquitetura

O bot de sinais utiliza uma arquitetura distribuída moderna:
- **Backend Web**: C# .NET 8 para API, autenticação e interface
- **Engine de Estratégias**: Python para análise técnica e ML
- **Comunicação**: RabbitMQ para eventos assíncronos
- **Banco de Dados**: PostgreSQL + Redis

## 📋 Pré-requisitos

### Ferramentas Necessárias
- **.NET 8 SDK** ou superior
- **Python 3.9+** com pip
- **Docker Desktop** para containers
- **Visual Studio 2022** ou **VS Code** 
- **Git** para versionamento

### APIs Externas (Obrigatórias)
- **Binance API** (gratuita) - Para criptomoedas
- **Alpha Vantage API** (gratuita) - Para ações e FIIs
- **Telegram Bot Token** - Para notificações

## 🚀 Setup Inicial

### 1. Clone do Repositório
```bash
git clone https://github.com/loribao/bot_sinais.git
cd bot_sinais
```

### 2. Configuração do Backend C#

#### 2.1. Criar Projeto ASP.NET Core
```bash
mkdir backend_csharp
cd backend_csharp

# Criar solution
dotnet new sln -n BotSinais

# Criar projetos
dotnet new webapi -n BotSinais.Api
dotnet new classlib -n BotSinais.Core
dotnet new classlib -n BotSinais.Infrastructure
dotnet new blazorserver -n BotSinais.Web

# Adicionar à solution
dotnet sln add BotSinais.Api/BotSinais.Api.csproj
dotnet sln add BotSinais.Core/BotSinais.Core.csproj
dotnet sln add BotSinais.Infrastructure/BotSinais.Infrastructure.csproj
dotnet sln add BotSinais.Web/BotSinais.Web.csproj
```

#### 2.2. Dependências NuGet
```bash
# BotSinais.Api
cd BotSinais.Api
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.SignalR
dotnet add package RabbitMQ.Client
dotnet add package Serilog.AspNetCore

# BotSinais.Infrastructure
cd ../BotSinais.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.PostgreSQL
dotnet add package StackExchange.Redis
dotnet add package RabbitMQ.Client
dotnet add package Newtonsoft.Json

# BotSinais.Web
cd ../BotSinais.Web
dotnet add package Microsoft.AspNetCore.SignalR.Client
```

#### 2.3. Configuração do appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=bot_sinais;User Id=postgres;Password=password123;",
    "Redis": "localhost:6379"
  },
  "RabbitMQ": {
    "ConnectionString": "amqp://admin:password@localhost:5672/",
    "CommandsQueue": "python_commands",
    "EventsQueue": "csharp_events"
  },
  "ExternalApis": {
    "AlphaVantage": {
      "ApiKey": "YOUR_ALPHA_VANTAGE_KEY",
      "BaseUrl": "https://www.alphavantage.co"
    },
    "Binance": {
      "BaseUrl": "https://api.binance.com"
    }
  },
  "Telegram": {
    "BotToken": "YOUR_TELEGRAM_BOT_TOKEN",
    "DefaultChatId": "YOUR_CHAT_ID"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 3. Configuração do Engine Python

#### 3.1. Estrutura de Diretórios
```bash
mkdir python_strategies
cd python_strategies

mkdir -p src/{core,data,indicators,strategies,ml,events}
mkdir -p tests
```

#### 3.2. Requirements.txt
```txt
# Data processing
pandas>=2.0.0
numpy>=1.24.0
aiohttp>=3.8.0
aiofiles>=23.0.0

# Technical analysis
ta-lib>=0.4.0
yfinance>=0.2.0

# Machine Learning
scikit-learn>=1.3.0
xgboost>=1.7.0
lightgbm>=4.0.0

# Messaging
aio-pika>=9.0.0
pika>=1.3.0

# Database
asyncpg>=0.28.0
redis>=4.5.0
sqlalchemy>=2.0.0

# Utilities
python-dotenv>=1.0.0
pydantic>=2.0.0
loguru>=0.7.0
asyncio-mqtt>=0.16.0

# Testing
pytest>=7.4.0
pytest-asyncio>=0.21.0
```

#### 3.3. Configuração Python (.env)
```env
# Database
POSTGRES_URL=postgresql://postgres:password123@localhost:5432/bot_sinais
REDIS_URL=redis://localhost:6379

# RabbitMQ
RABBITMQ_URL=amqp://admin:password@localhost:5672/
COMMANDS_QUEUE=python_commands
EVENTS_QUEUE=csharp_events

# External APIs
ALPHA_VANTAGE_API_KEY=YOUR_ALPHA_VANTAGE_KEY
BINANCE_API_KEY=YOUR_BINANCE_KEY
BINANCE_SECRET_KEY=YOUR_BINANCE_SECRET

# Telegram
TELEGRAM_BOT_TOKEN=YOUR_TELEGRAM_BOT_TOKEN
TELEGRAM_CHAT_ID=YOUR_CHAT_ID

# Logging
LOG_LEVEL=INFO
LOG_FILE=logs/strategy_engine.log
```

### 4. Docker Compose Setup

#### 4.1. docker-compose.yml
```yaml
version: '3.8'

services:
  # Infrastructure
  postgres:
    image: postgres:15-alpine
    environment:
      POSTGRES_DB: bot_sinais
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: password123
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 10s
      timeout: 5s
      retries: 5

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 10s
      timeout: 3s
      retries: 3

  rabbitmq:
    image: rabbitmq:3-management-alpine
    environment:
      RABBITMQ_DEFAULT_USER: admin
      RABBITMQ_DEFAULT_PASS: password
    ports:
      - "5672:5672"    # AMQP
      - "15672:15672"  # Management UI
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq
    healthcheck:
      test: rabbitmq-diagnostics -q ping
      interval: 30s
      timeout: 30s
      retries: 3

volumes:
  postgres_data:
  redis_data:
  rabbitmq_data:
```

### 5. Comandos de Execução

#### 5.1. Desenvolvimento Local
```bash
# 1. Subir infraestrutura
docker-compose up -d postgres redis rabbitmq

# 2. Backend C# (Terminal 1)
cd backend_csharp/BotSinais.Api
dotnet watch run

# 3. Engine Python (Terminal 2)
cd python_strategies
python -m venv venv
source venv/bin/activate  # Linux/Mac
# ou
venv\Scripts\activate     # Windows
pip install -r requirements.txt
python main.py

# 4. Frontend Web (Terminal 3)
cd backend_csharp/BotSinais.Web
dotnet watch run
```

#### 5.2. Produção com Docker
```bash
# Build e deploy completo
docker-compose -f docker-compose.prod.yml up -d
```

### 6. Migrações de Banco

#### 6.1. Entity Framework (C#)
```bash
cd backend_csharp/BotSinais.Api

# Adicionar migração
dotnet ef migrations add InitialCreate

# Aplicar ao banco
dotnet ef database update
```

#### 6.2. Schema Principal
```sql
-- Tabelas principais
CREATE TABLE Signals (
    Id UUID PRIMARY KEY,
    Asset VARCHAR(20) NOT NULL,
    Direction VARCHAR(10) NOT NULL,
    Confidence DECIMAL(5,4) NOT NULL,
    EntryPrice DECIMAL(18,8) NOT NULL,
    StopLoss DECIMAL(18,8),
    TakeProfit DECIMAL(18,8),
    Strategy VARCHAR(50) NOT NULL,
    Timeframe VARCHAR(10) NOT NULL,
    Status VARCHAR(20) NOT NULL,
    CreatedAt TIMESTAMP WITH TIME ZONE NOT NULL,
    UpdatedAt TIMESTAMP WITH TIME ZONE,
    Indicators JSONB
);

CREATE TABLE SignalResults (
    Id UUID PRIMARY KEY,
    SignalId UUID REFERENCES Signals(Id),
    Result VARCHAR(20) NOT NULL,
    ExitPrice DECIMAL(18,8),
    PnL DECIMAL(10,4),
    ExitTime TIMESTAMP WITH TIME ZONE
);

-- Índices para performance
CREATE INDEX idx_signals_asset_created ON Signals(Asset, CreatedAt);
CREATE INDEX idx_signals_status ON Signals(Status);
CREATE INDEX idx_signals_strategy ON Signals(Strategy);
```

### 7. Monitoramento e Logs

#### 7.1. URLs de Acesso
- **API C#**: http://localhost:5000
- **Frontend Web**: http://localhost:5001
- **RabbitMQ Management**: http://localhost:15672 (admin/password)
- **PostgreSQL**: localhost:5432
- **Redis**: localhost:6379

#### 7.2. Health Checks
```bash
# API Status
curl http://localhost:5000/health

# RabbitMQ Status
curl -u admin:password http://localhost:15672/api/overview

# Python Engine Status
# Verificar logs no container ou arquivo de log
```

### 8. Testes

#### 8.1. Testes C#
```bash
cd backend_csharp
dotnet test
```

#### 8.2. Testes Python
```bash
cd python_strategies
pytest tests/ -v
```

### 9. Deployment

#### 9.1. Azure Container Instances
```bash
# Build images
docker build -t botsinais-api ./backend_csharp
docker build -t botsinais-python ./python_strategies

# Deploy to Azure
az container create --resource-group myResourceGroup \
  --name bot-sinais --image botsinais-api
```

#### 9.2. AWS ECS
```bash
# Build and push to ECR
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 123456789012.dkr.ecr.us-east-1.amazonaws.com

docker build -t botsinais-api .
docker tag botsinais-api:latest 123456789012.dkr.ecr.us-east-1.amazonaws.com/botsinais-api:latest
docker push 123456789012.dkr.ecr.us-east-1.amazonaws.com/botsinais-api:latest
```

## 🔧 Troubleshooting

### Problemas Comuns

#### 1. RabbitMQ Connection Failed
```bash
# Verificar se RabbitMQ está rodando
docker ps | grep rabbitmq

# Verificar logs
docker logs bot_rabbitmq
```

#### 2. PostgreSQL Connection Issues
```bash
# Testar conexão
psql -h localhost -U postgres -d bot_sinais

# Verificar permissões
docker exec -it bot_postgres psql -U postgres -c "\l"
```

#### 3. Python Dependencies Issues
```bash
# Reinstalar dependências
pip uninstall -r requirements.txt -y
pip install -r requirements.txt --force-reinstall
```

#### 4. .NET Build Errors
```bash
# Limpar e rebuild
dotnet clean
dotnet restore
dotnet build
```

## 📊 Monitoramento de Performance

### Métricas Importantes
- **Latência de Sinais**: < 500ms
- **Throughput**: > 100 sinais/minuto
- **Uptime**: > 99.9%
- **Accuracy**: > 70% win rate

### Logs Estruturados
- **C#**: Serilog com structured logging
- **Python**: Loguru com JSON format
- **Centralização**: ELK Stack ou Azure Monitor

## 🔐 Segurança

### Checklist de Segurança
- [ ] API Keys em variáveis de ambiente
- [ ] HTTPS obrigatório em produção
- [ ] JWT com expiração adequada
- [ ] Rate limiting configurado
- [ ] Validação de input rigorosa
- [ ] Logs sanitizados (sem dados sensíveis)

Esta configuração fornece uma base sólida e escalável para o bot de sinais, permitindo desenvolvimento ágil e deployment confiável.
