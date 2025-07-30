# Sistema de Sinais de Trading - Modelagem de Domínio

## Visão Geral

Este projeto implementa um sistema de sinais de trading baseado em **Domain-Driven Design (DDD)** com arquitetura modular usando **.NET 9.0** e **.NET Aspire**. O sistema é organizado em contextos delimitados que se comunicam através de eventos usando **MassTransit** e **RabbitMQ**.

## 🏗️ Arquitetura Atual

### **Projetos Implementados:**
- **`BotSinais.Domain`** - Domain puro com entidades e regras de negócio
- **`BotSinais.Infrastructure`** - Implementações e integrações externas (organizadas por módulos)
- **`BotSinais.ApiService`** - APIs REST (configuração mínima usando Infrastructure.Shared)
- **`BotSinais.Web`** - Interface web Blazor
- **`BotSinais.AppHost`** - Orquestração .NET Aspire
- **`BotSinais.ServiceDefaults`** - Configurações padrão Aspire
- **`BotSinais.Tests`** - Testes unitários e de integração

### **Observação sobre Application Layer:**
Atualmente **não há uma camada Application separada**. As responsabilidades de aplicação estão distribuídas entre:
- **Controllers** (Infrastructure) - Endpoints HTTP
- **Services** (Infrastructure) - Lógica de aplicação
- **Handlers** (Infrastructure) - Processamento de eventos

## 🎯 Contextos Delimitados Implementados

### 1. � Contexto de Autenticação (`Auth`)

**Status**: **Infraestrutura implementada** (sem entidades de domínio próprias)
**Responsabilidade**: Autenticação JWT via Keycloak, autorização e gerenciamento de sessões.

#### Implementação Atual:
- **`AuthController`**: Controller completo com 10+ endpoints
- **Keycloak Integration**: Configuração completa para JWT
- **Fluxos Suportados**: Authorization Code, ROPC (Resource Owner Password Credentials)
- **Validação de Token**: Local e via Keycloak Token Introspection

#### Características:
- Autenticação direta via username/password
- Validação JWT local e remota
- Informações do usuário autenticado
- Logout com limpeza de sessão
- Testes HTTP completos

### 2. �📊 Contexto de Gerenciamento de Dados (`DataManagement`)

**Status**: **✅ Implementado completamente**
**Responsabilidade**: Coleta, armazenamento e validação de dados de mercado.

#### Entidades Principais:
- **`Instrument`**: Representa instrumentos financeiros (ações, forex, crypto, etc.)
- **`MarketData`**: Dados OHLCV com timeframes configuráveis
- **`DataSource`**: Fontes de dados (REST, WebSocket, arquivos)
- **`TradingSession`**: Sessões de negociação com horários específicos

#### Características:
- Suporte a múltiplos timeframes (M1, M5, M15, H1, D1, etc.)
- Validação de integridade dos dados
- Múltiplas fontes de dados com priorização
- Dados históricos e em tempo real

### 3. 📈 Contexto de Sinais/Trading (`Signals`)

**Status**: **✅ Implementado completamente**
**Responsabilidade**: Geração, execução e gerenciamento de sinais de trading.

#### Entidades Principais:
- **`TradingSignal`**: Sinal de trading com direção, preços e confiança
- **`SignalExecution`**: Execução real do sinal no mercado
- **`Portfolio`**: Carteira de investimentos com gestão de risco
- **`Position`**: Posições abertas/fechadas em instrumentos
- **`PositionTrade`**: Trades individuais de uma posição

#### Características:
- Sinais com múltiplos níveis de confiança
- Gestão de risco por carteira e por trade
- Tracking de performance em tempo real
- Stop-loss e take-profit automáticos

### 4. 🔧 Contexto de Estratégias (`Strategies`)

**Status**: **✅ Implementado completamente**
**Responsabilidade**: Criação, execução e backtesting de estratégias multi-linguagem.

#### Entidades Principais:
- **`Strategy`**: Estratégia de trading com código-fonte
- **`StrategyExecution`**: Execução de estratégia em produção
- **`StrategyBacktest`**: Resultado de backtests históricos
- **`StrategyTemplate`**: Templates reutilizáveis de estratégias
- **`ExecutionEngine`**: Engines para C#, Python e Julia

#### Características:
- Suporte a **C#**, **Python** e **Julia**
- Sistema de templates para estratégias
- Backtesting com métricas detalhadas
- Execução isolada por linguagem

## 🔄 Comunicação por Eventos (MassTransit)

### **Status da Implementação**: **🏗️ Estrutura preparada**

Os eventos estão organizados em pastas por contexto no diretório `Shared/Events/`:

#### **Eventos por Contexto:**

**📊 DataManagement** (`Events/DataManagement/`):
- `MarketDataReceivedEvent` - Novos dados de mercado
- `MarketDataUpdatedEvent` - Atualizações de dados
- `InstrumentAddedEvent` - Novos instrumentos
- `HistoricalDataLoadedEvent` - Dados históricos carregados

**📈 Signals** (`Events/Signals/`):
- `SignalGeneratedEvent` - Novo sinal criado
- `SignalExecutedEvent` - Sinal executado
- `SignalStatusChangedEvent` - Mudança de status
- `PositionOpenedEvent` - Posição aberta
- `PositionClosedEvent` - Posição fechada

**🔧 Strategies** (`Events/Strategies/`):
- `StrategyCreatedEvent` - Nova estratégia
- `StrategyExecutionStartedEvent` - Execução iniciada
- `StrategyExecutionCompletedEvent` - Execução finalizada
- `BacktestCompletedEvent` - Backtest finalizado

**🔧 System** (`Events/System/`):
- `SystemErrorEvent` - Erros do sistema

### Configuração do MassTransit:

```csharp
// Registro no DI Container
services.AddMassTransit(x =>
{
    // Registrar consumidores
    x.AddConsumers(Assembly.GetExecutingAssembly());
    
    // Configurar RabbitMQ
    MassTransitConfiguration.ConfigureBus(x, connectionString);
});
```

## 🏗️ Estrutura Real de Pastas

```
BotSinais.Domain/
├── Shared/                           # ✅ Classes base e tipos compartilhados
│   ├── BaseEntity.cs                # ✅ Entidade base com ID, timestamps
│   ├── Enums.cs                     # ✅ Enumerações principais
│   ├── ValueObjects.cs              # ✅ Value Objects principais
│   ├── ICommand.cs                  # ✅ Interface para comandos
│   ├── IQuery.cs                    # ✅ Interface para queries
│   ├── IRepositoryBase.cs           # ✅ Interface base para repositórios
│   ├── Entities/                    # ✅ Entidades compartilhadas
│   ├── Enums/                       # ✅ Enumerações especializadas
│   │   ├── ExecutionLanguage.cs    # ✅ Linguagens de execução
│   │   ├── InstrumentType.cs       # ✅ Tipos de instrumentos
│   │   ├── SignalStatus.cs         # ✅ Status de sinais
│   │   ├── StrategyType.cs         # ✅ Tipos de estratégias
│   │   ├── TimeFrame.cs            # ✅ Timeframes de dados
│   │   └── TradeDirection.cs       # ✅ Direções de trade
│   ├── ValueObjects/               # ✅ Value Objects especializados
│   │   ├── Price.cs                # ✅ Preço com moeda
│   │   ├── Symbol.cs               # ✅ Símbolo com exchange
│   │   └── Volume.cs               # ✅ Volume de negociação
│   ├── Events/                     # ✅ Sistema de eventos
│   │   ├── DomainEventBase.cs      # ✅ Evento base
│   │   ├── DomainEvents.cs         # ✅ Índice de eventos
│   │   ├── IDomainEventPublisher.cs # ✅ Interface do publisher
│   │   ├── EventInfrastructure.cs  # ✅ Configuração MassTransit
│   │   ├── EventHandlerExamples.cs # ✅ Exemplos de handlers
│   │   ├── DataManagement/         # ✅ Eventos de dados
│   │   ├── Signals/                # ✅ Eventos de sinais
│   │   ├── Strategies/             # ✅ Eventos de estratégias
│   │   └── System/                 # ✅ Eventos do sistema
│   └── Interfaces/                 # ✅ Interfaces compartilhadas
├── Modules/                        # ✅ Contextos delimitados
│   ├── Auth/                       # 🔄 Vazio (implementado em Infrastructure)
│   ├── DataManagement/             # ✅ Contexto de dados
│   │   ├── Entities/               # ✅ Entidades do contexto
│   │   │   ├── Instrument.cs       # ✅ Instrumentos financeiros
│   │   │   ├── MarketData.cs       # ✅ Dados OHLCV
│   │   │   ├── DataSource.cs       # ✅ Fontes de dados
│   │   │   ├── TradingSession.cs   # ✅ Sessões de trading
│   │   │   └── DataSourceInstrument.cs # ✅ Relacionamento
│   │   └── Interfaces/             # ✅ Contratos de dados
│   ├── Signals/                    # ✅ Contexto de sinais
│   │   ├── Entities/               # ✅ Entidades do contexto
│   │   │   ├── TradingSignal.cs    # ✅ Sinais de trading
│   │   │   ├── SignalExecution.cs  # ✅ Execuções de sinais
│   │   │   ├── SignalUpdate.cs     # ✅ Atualizações de sinais
│   │   │   ├── Portfolio.cs        # ✅ Carteiras de investimento
│   │   │   ├── Position.cs         # ✅ Posições em instrumentos
│   │   │   ├── PositionTrade.cs    # ✅ Trades individuais
│   │   │   └── PortfolioSignal.cs  # ✅ Relacionamento
│   │   └── Interfaces/             # ✅ Contratos de sinais
│   └── Strategies/                 # ✅ Contexto de estratégias
│       ├── Entities/               # ✅ Entidades do contexto
│       │   ├── Strategy.cs         # ✅ Estratégias de trading
│       │   ├── StrategyExecution.cs # ✅ Execuções de estratégias
│       │   ├── StrategyBacktest.cs # ✅ Backtests de estratégias
│       │   ├── BacktestTrade.cs    # ✅ Trades de backtest
│       │   ├── StrategyInstrument.cs # ✅ Relacionamento
│       │   ├── StrategyTemplate.cs # ✅ Templates de estratégias
│       │   └── ExecutionEngine.cs  # ✅ Engines de execução
│       └── Interfaces/             # ✅ Contratos de estratégias
└── GlobalEventUsings.cs            # ✅ Usings globais
```

## 🏛️ Estrutura de Infraestrutura (BotSinais.Infrastructure)

### **Organização Modular Implementada:**

```
BotSinais.Infrastructure/
├── Modules/                        # ✅ Organização por contextos DDD
│   ├── Auth/                       # ✅ Módulo de autenticação
│   │   ├── Controllers/            # ✅ AuthController.cs + .http
│   │   ├── Services/              # ✅ Serviços de auth
│   │   ├── ServiceCollectionExtensions.cs  # ✅ DI do módulo
│   │   └── WebApplicationExtensions.cs     # ✅ Pipeline do módulo
│   │
│   ├── Signals/                    # ✅ Módulo de sinais
│   │   ├── Controllers/            # ✅ TradingSignalsController.cs + .http
│   │   ├── Handlers/              # ✅ Event handlers
│   │   └── [configurações...]     # ✅ Configurações do módulo
│   │
│   ├── DataManagement/             # 🏗️ Preparado para implementação
│   ├── Strategies/                 # 🏗️ Preparado para implementação
│   │
│   └── Shared/                     # ✅ Configuração unificada
│       ├── ServiceCollectionExtensions.cs  # ✅ DI de todos os módulos
│       └── WebApplicationExtensions.cs     # ✅ Pipeline unificado
└── [outros arquivos...]           # ✅ Configurações gerais
```

### **Características da Implementação:**

- ✅ **Configuração Modular**: Cada módulo tem suas próprias extensões de DI
- ✅ **Pipeline Unificado**: Shared coordena todos os módulos
- ✅ **Testes HTTP**: Cada controller tem arquivo .http para testes
- ✅ **Aspire Integration**: Totalmente integrado com .NET Aspire

## � Características Importantes da Implementação

### **Value Objects Implementados:**
- ✅ **`Price`**: Encapsula valor monetário e moeda
- ✅ **`Volume`**: Representa volume de negociação  
- ✅ **`Symbol`**: Código do instrumento + exchange

### **Enumerações Especializadas:**
- ✅ **`InstrumentType`**: Stock, Forex, Crypto, Commodity, Index, Future, Option, Bond, ETF
- ✅ **`TimeFrame`**: M1, M5, M15, M30, H1, H4, D1, W1, MN1
- ✅ **`TradeDirection`**: Buy, Sell, Hold
- ✅ **`SignalStatus`**: Created, Active, Executed, Cancelled, Expired
- ✅ **`ExecutionLanguage`**: CSharp, Python, Julia
- ✅ **`StrategyType`**: Manual, Automated, Hybrid

### **Padrões Arquiteturais Implementados:**
- ✅ **Domain-Driven Design**: Contextos delimitados bem definidos
- ✅ **Repository Pattern**: Interfaces para acesso a dados
- ✅ **Domain Events**: Sistema de eventos organizado por contexto
- ✅ **Modular Architecture**: Infrastructure organizada por módulos DDD
- ✅ **CQRS-Ready**: Interfaces ICommand e IQuery preparadas

### **Tecnologias e Frameworks:**
- ✅ **.NET 9.0** - Framework principal
- ✅ **.NET Aspire** - Orquestração e observabilidade  
- ✅ **MassTransit** - Preparado para RabbitMQ
- ✅ **Keycloak** - Autenticação JWT implementada
- 🏗️ **Entity Framework Core** - Preparado para implementação
- 🏗️ **PostgreSQL** - Configurado via Aspire

## � Fluxo de Execução Planejado

### **1. Autenticação (✅ Implementado)**
```
1. Login via Keycloak → JWT Token
2. Validação de Token → Acesso autorizado
3. APIs protegidas → Operações autenticadas
```

### **2. Fluxo de Dados (🏗️ Preparado)**
```
1. Coleta de Dados → DataSource recebe dados de mercado
2. Evento de Dados → Publica MarketDataReceivedEvent
3. Validação → Verifica integridade dos dados
4. Armazenamento → Persiste dados válidos
```

### **3. Fluxo de Sinais (🏗️ Preparado)**
```
1. Análise → Handler processa dados e gera sinais
2. Evento de Sinal → Publica SignalGeneratedEvent  
3. Validação → Verifica regras de risco
4. Execução → Sistema executa sinal baseado em portfolio
5. Evento de Execução → Publica SignalExecutedEvent
6. Atualização → Posições e performance atualizadas
```

### **4. Fluxo de Estratégias (🏗️ Preparado)**
```
1. Criação → Desenvolve estratégia em C#/Python/Julia
2. Compilação → Valida e compila código
3. Backtest → Testa estratégia com dados históricos
4. Execução → Estratégia ativa gera sinais automaticamente
5. Monitoramento → Acompanha performance em tempo real
```

## 🧪 **Status de Testes**

### **Testes HTTP Implementados:**
- ✅ **AuthController.http** - Autenticação completa (10+ endpoints)
- ✅ **TradingSignalsController.http** - APIs de sinais básicas
- 🏗️ **DataManagement** - Preparado para implementação
- 🏗️ **Strategies** - Preparado para implementação

### **Arquivos de Teste por Contexto:**
```
Infrastructure/Modules/
├── Auth/Controllers/AuthController.http           # ✅ Funcional
├── Signals/Controllers/TradingSignals.http        # ✅ Básico
├── DataManagement/Controllers/[futuro].http       # 🏗️ Planejado
└── Strategies/Controllers/[futuro].http           # 🏗️ Planejado
```

## 🎯 **Próximos Passos de Implementação**

### **Prioridade Alta (Próximas Sprints):**
1. ✅ **Autenticação** - Concluído com validação de token
2. 🏗️ **Entity Framework Context** - Implementar DbContext por módulo
3. 🏗️ **Repositories** - Implementar repositórios com EF Core
4. 🏗️ **APIs DataManagement** - Endpoints para instrumentos e dados
5. 🏗️ **APIs Signals** - Endpoints completos para sinais

### **Prioridade Média:**
6. 🏗️ **MassTransit Integration** - Ativar sistema de eventos
7. 🏗️ **Strategy Execution Engines** - Implementar engines C#/Python/Julia
8. 🏗️ **Backtesting Service** - Sistema de backtests
9. 🏗️ **Performance Analytics** - Métricas e relatórios

### **Prioridade Baixa (Futuro):**
10. 🏗️ **Application Layer** - Separar lógica de aplicação dos controllers
11. 🏗️ **Event Sourcing** - Para auditoria completa
12. 🏗️ **CQRS** - Separação comando/query
13. 🏗️ **Microservices** - Divisão por contexto se necessário

## 🚀 **Como Executar o Sistema Atual**

### **Pré-requisitos:**
- .NET 9.0 SDK
- Docker Desktop (para Keycloak)
- VS Code com REST Client extension

### **Executar:**
```bash
cd BotSinais.AppHost
dotnet run
```

### **Acessar:**
- **Dashboard Aspire**: https://localhost:17053
- **API Base**: https://localhost:17053/api
- **Keycloak**: http://localhost:8080

### **Testar Autenticação:**
1. Abrir `Infrastructure/Modules/Auth/Controllers/AuthController.http`
2. Executar "Autenticação Direta"
3. Verificar se recebeu JWT token
4. Testar endpoints de validação

---

**📅 Última atualização**: 30 de julho de 2025  
**🎯 Status**: Domain modelado, Auth implementado, Infrastructure modular preparada  
**🚀 Próximo**: Implementar Entity Framework e APIs completas
