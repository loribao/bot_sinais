# GitHub Copilot Instructions

Este documento fornece instruções específicas para o GitHub Copilot ao trabalhar no projeto **Bot Sinais - Sistema de Trading**.

## 🎯 Sobre o Projeto

Este é um sistema de sinais de trading baseado em **Domain-Driven Design (DDD)** com arquitetura orientada a eventos usando **MassTransit** e **RabbitMQ**. O sistema utiliza **.NET Aspire** para orquestração da infraestrutura e é dividido em três contextos delimitados principais.

## 🏗️ Arquitetura do Sistema

### Contextos Delimitados

1. **📊 DataManagement** - Gerenciamento de dados de mercado
2. **📈 Signals** - Geração e execução de sinais de trading  
3. **🔧 Strategies** - Criação e execução de estratégias (C#, Python, Julia)

### Tecnologias Principais
- **.NET 9.0** (C#)
- **.NET Aspire** (orquestração e observabilidade)
- **MassTransit 8.2.0** com RabbitMQ
- **Entity Framework Core** (para persistência)
- **ASP.NET Core** (APIs e Web)
- **Keycloak** (autenticação e autorização)
- **PostgreSQL** (banco de dados)
- **Python/Julia** (engines de estratégias)

### Arquitetura de Projetos
- **BotSinais.AppHost** - .NET Aspire orchestrator (infraestrutura)
- **BotSinais.ApiService** - API endpoints (mínima configuração)
- **BotSinais.Web** - Interface web (Blazor)
- **BotSinais.Infrastructure** - **TODAS as configurações e injeções de dependência**
- **BotSinais.Domain** - Domínio puro (sem dependências)
- **BotSinais.ServiceDefaults** - Configurações padrão Aspire

## 📝 Convenções de Código

### Namespaces
```csharp
// DOMAIN - Sem dependências externas
BotSinais.Domain.Shared                         // Classes base e tipos compartilhados
BotSinais.Domain.Shared.Events                  // Eventos de domínio e interfaces
BotSinais.Domain.Shared.ValueObjects            // Value Objects (Price, Volume, Symbol)
BotSinais.Domain.Shared.Enums                   // Enumerações (InstrumentType, TimeFrame, etc.)
BotSinais.Domain.Modules.DataManagement.Entities    // Entidades de dados
BotSinais.Domain.Modules.DataManagement.Interfaces  // Interfaces de dados
BotSinais.Domain.Modules.Signals.Entities           // Entidades de sinais
BotSinais.Domain.Modules.Signals.Interfaces         // Interfaces de sinais
BotSinais.Domain.Modules.Strategies.Entities        // Entidades de estratégias
BotSinais.Domain.Modules.Strategies.Interfaces      // Interfaces de estratégias

// INFRASTRUCTURE - Implementações organizadas por módulos
BotSinais.Infrastructure.Modules.Auth                   // Autenticação Keycloak, middlewares, controllers
BotSinais.Infrastructure.Modules.Signals                // Controllers de sinais, handlers MassTransit
BotSinais.Infrastructure.Modules.DataManagement         // Entity Framework, repositórios, controllers de dados
BotSinais.Infrastructure.Modules.Strategies             // Engines C#/Python/Julia, controllers de estratégias
BotSinais.Infrastructure.Shared                         // Configurações unificadas de todos os módulos
```

### Padrões de Nomenclatura

#### Entidades
- Use PascalCase
- Nomes descritivos: `TradingSignal`, `MarketData`, `StrategyExecution`
- Herdem de `BaseEntity` quando apropriado

#### Value Objects
- Use `record` quando possível
- Exemplos: `Price`, `Volume`, `Symbol`
- Inclua validação no construtor

#### Eventos
- Sufixo `Event`: `SignalGeneratedEvent`, `MarketDataReceivedEvent`
- Herdem de `DomainEvent`
- Use `record` para imutabilidade

#### Interfaces
- Prefixo `I`: `IStrategyRepository`, `ISignalExecutionService`
- Repositórios: `I{Entity}Repository`
- Serviços: `I{Domain}Service`

### Estrutura de Entidades

```csharp
public class ExampleEntity : BaseEntity, IVersionedEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    public int Version { get; set; } = 1;
    
    // Relacionamentos
    public virtual ICollection<RelatedEntity> Items { get; set; } = new List<RelatedEntity>();
}
```

## 🔄 Padrões de Eventos

### Publicação de Eventos
```csharp
// Sempre use IDomainEventPublisher
await _eventPublisher.PublishAsync(new SignalGeneratedEvent
{
    SignalId = signal.Id,
    InstrumentId = signal.InstrumentId,
    // ... outras propriedades
}, cancellationToken);
```

### Consumo de Eventos
```csharp
public class SignalEventHandler : DomainEventHandler<SignalGeneratedEvent>
{
    public SignalEventHandler(ILogger<SignalEventHandler> logger) : base(logger) { }
    
    protected override async Task HandleAsync(SignalGeneratedEvent @event, CancellationToken cancellationToken)
    {
        // Implementação do handler
    }
}
```

## 🛠️ Padrões de Implementação

### Configuração de Infraestrutura (.NET Aspire)
```csharp
// AppHost.cs - Orquestração da infraestrutura
var builder = DistributedApplication.CreateBuilder(args);

// Recursos de infraestrutura
var postgres = builder.AddPostgres("postgres");
var rabbitmq = builder.AddRabbitMQ("messaging");
var keycloak = builder.AddKeycloak("keycloak", 8080);

// Projetos da aplicação
var apiService = builder.AddProject<Projects.BotSinais_ApiService>("apiservice")
    .WithReference(postgres)
    .WithReference(rabbitmq)
    .WithReference(keycloak);
```

### Configuração Centralizada em Infrastructure (Modular)
**IMPORTANTE**: Todas as configurações de DI, controllers, minimal APIs e middlewares devem ser organizadas por **módulos** no projeto **BotSinais.Infrastructure**, seguindo os contextos delimitados do DDD.

#### Estrutura Modular
```
BotSinais.Infrastructure/
├── Modules/
│   ├── Auth/                    # Autenticação e autorização
│   ├── Signals/                 # Sinais de trading  
│   ├── DataManagement/          # Dados de mercado
│   └── Strategies/              # Estratégias de trading
└── Shared/                      # Configurações unificadas
```

#### Configuração por Módulo
```csharp
// BotSinais.Infrastructure/Modules/Auth/ServiceCollectionExtensions.cs
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthKeycloak(configuration);
        // Configurações específicas do módulo Auth
        return services;
    }
}

// BotSinais.Infrastructure/Modules/Auth/WebApplicationExtensions.cs
public static class WebApplicationExtensions
{
    public static WebApplication UseAuthModule(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
```

#### Configuração Unificada (Shared)
```csharp
// BotSinais.Infrastructure/Shared/ServiceCollectionExtensions.cs
public static IServiceCollection AddBotSinaisInfrastructure(this IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    
    // Todos os módulos
    services.AddAuthModule(configuration);
    services.AddSignalsModule(configuration);
    services.AddDataManagementModule(configuration);
    services.AddStrategiesModule(configuration);
    
    return services;
}
```

### Projeto ApiService Mínimo
O projeto **BotSinais.ApiService** deve ter configuração mínima usando o módulo **Shared**:

```csharp
// Program.cs - ApiService (MÍNIMO)
using BotSinais.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

// Configuração centralizada unificada (todos os módulos)
builder.Services.AddBotSinaisInfrastructure(builder.Configuration);

var app = builder.Build();

// Pipeline centralizado unificado (todos os módulos)
app.ConfigureBotSinaisPipeline();
app.MapDefaultEndpoints();
app.Run();
```

### Repositórios
```csharp
public interface IExampleRepository
{
    Task<Example?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Example>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<Example> CreateAsync(Example entity, CancellationToken cancellationToken = default);
    Task<Example> UpdateAsync(Example entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
```

### Serviços de Domínio
```csharp
public interface IExampleService
{
    Task<Result> ProcessAsync(Guid id, Dictionary<string, object> parameters, CancellationToken cancellationToken = default);
    Task<bool> ValidateAsync(Example entity, CancellationToken cancellationToken = default);
}
```

## 💰 Contexto Financeiro

### Tipos de Instrumentos
- **Stock** - Ações
- **Forex** - Câmbio
- **Crypto** - Criptomoedas
- **Commodity** - Commodities
- **Index** - Índices
- **Future** - Futuros
- **Option** - Opções

### Timeframes Suportados
- **M1, M5, M15, M30** - Minutos
- **H1, H4** - Horas  
- **D1** - Diário
- **W1** - Semanal
- **MN1** - Mensal

### Direções de Trading
- **Buy** - Compra
- **Sell** - Venda
- **Hold** - Manter posição

## 🔧 Execução Multi-Linguagem

### Linguagens Suportadas
```csharp
public enum ExecutionLanguage
{
    CSharp,  // Estratégias em C#
    Python,  // Estratégias em Python
    Julia    // Estratégias em Julia
}
```

### Estrutura de Estratégias
- Código fonte armazenado como string
- Parâmetros configuráveis via Dictionary
- Templates reutilizáveis disponíveis
- Isolamento por ExecutionEngine

## 📊 Métricas e Performance

### Métricas de Backtest
- **TotalReturn** - Retorno total
- **SharpeRatio** - Índice Sharpe
- **MaxDrawdown** - Maior perda
- **WinRate** - Taxa de acerto
- **ProfitFactor** - Fator de lucro

### Gestão de Risco
- **MaxRiskPerTrade** - Risco máximo por operação (padrão: 2%)
- **MaxPortfolioRisk** - Risco máximo do portfolio (padrão: 20%)

## 🚨 Tratamento de Erros

### Eventos de Erro
```csharp
await _eventPublisher.PublishAsync(new SystemErrorEvent
{
    Component = "ComponentName",
    ErrorType = "ErrorType",
    Message = ex.Message,
    StackTrace = ex.StackTrace,
    Context = new Dictionary<string, object> { /* contexto */ }
}, cancellationToken);
```

### Validações
- Sempre validar entradas
- Usar Data Annotations onde apropriado
- Implementar validações de negócio nos serviços

## 🧪 Testes

### Convenções de Teste
- Use AAA pattern (Arrange, Act, Assert)
- Nomes descritivos: `Should_CreateSignal_When_ValidParametersProvided`
- Mock dependências externas
- Teste cenários de erro

### Exemplo de Teste
```csharp
[Fact]
public async Task Should_GenerateSignal_When_MarketDataReceived()
{
    // Arrange
    var marketData = new MarketDataReceivedEvent { /* dados */ };
    
    // Act
    var result = await _service.ProcessAsync(marketData);
    
    // Assert
    result.Should().NotBeNull();
    result.Direction.Should().Be(TradeDirection.Buy);
}
```

## 📁 Estrutura de Arquivos

```
src-cs/
├── BotSinais.Domain/                       # ⚡ Domain puro - sem dependências externas
│   ├── Shared/                             # Arquivos compartilhados entre contextos
│   │   ├── Entities/                       # Entidades base (BaseEntity, etc.)
│   │   ├── Interfaces/                     # Interfaces base (ICommand, IQuery, IRepositoryBase)
│   │   ├── ValueObjects/                   # Value Objects - um arquivo por tipo
│   │   │   ├── Price.cs
│   │   │   ├── Volume.cs
│   │   │   └── Symbol.cs
│   │   ├── Enums/                          # Enumerações - um arquivo por enum
│   │   │   ├── InstrumentType.cs
│   │   │   ├── TimeFrame.cs
│   │   │   ├── TradeDirection.cs
│   │   │   ├── SignalStatus.cs
│   │   │   ├── StrategyType.cs
│   │   │   └── ExecutionLanguage.cs
│   │   └── Events/                         # Eventos de domínio (abstrações puras)
│   │       ├── DomainEvents.cs             # Definições dos eventos
│   │       └── IDomainEventPublisher.cs    # Interface para publicação
│   └── Modules/                            # Contextos delimitados (Bounded Contexts)
│       ├── DataManagement/                 # Contexto de dados
│       │   ├── Entities/                   # Uma entidade por arquivo
│       │   │   ├── Instrument.cs
│       │   │   ├── MarketData.cs
│       │   │   └── ...
│       │   └── Interfaces/                 # Uma interface por arquivo
│       │       ├── IInstrumentRepository.cs
│       │       ├── IMarketDataRepository.cs
│       │       └── ...
│       ├── Signals/                        # Contexto de sinais
│       │   ├── Entities/                   # Uma entidade por arquivo
│       │   │   ├── TradingSignal.cs
│       │   │   ├── Portfolio.cs
│       │   │   └── ...
│       │   └── Interfaces/                 # Uma interface por arquivo
│       │       ├── ITradingSignalRepository.cs
│       │       ├── IPortfolioRepository.cs
│       │       └── ...
│       └── Strategies/                     # Contexto de estratégias
│           ├── Entities/                   # Uma entidade por arquivo
│           │   ├── Strategy.cs
│           │   ├── StrategyExecution.cs
│           │   └── ...
│           └── Interfaces/                 # Uma interface por arquivo
│               ├── IStrategyRepository.cs
│               ├── IStrategyExecutionService.cs
│               └── ...
├── BotSinais.Infrastructure/               # 🔌 Implementações e dependências externas
│   ├── Modules/                            # Módulos organizados por contexto delimitado
│   │   ├── Auth/                           # Módulo de autenticação
│   │   │   └── Controllers/                # Controllers e testes HTTP
│   │   │       ├── AuthController.cs      # Controller de autenticação
│   │   │       └── AuthController.http    # ⭐ Testes HTTP do controller
│   │   ├── Signals/                        # Módulo de sinais
│   │   │   └── Controllers/                # Controllers e testes HTTP
│   │   │       ├── TradingSignalsController.cs
│   │   │       └── TradingSignals.http    # ⭐ Testes HTTP do controller
│   │   └── [outros módulos...]
│   └── Events/                             # Infraestrutura de eventos
│       ├── EventInfrastructure.cs         # Implementação MassTransit
│       └── EventHandlerExamples.cs        # Exemplos de handlers
├── BotSinais.Application/                  # Casos de uso e handlers
├── BotSinais.ApiService/                   # API REST
├── BotSinais.Web/                         # Interface web
└── BotSinais.Tests/                       # Testes unitários
```

## 🔒 Segurança

### Configurações Sensíveis
- Use `IConfiguration` para configurações
- Nunca hardcode connection strings
- Use User Secrets em desenvolvimento
- Variables de ambiente em produção

### Validação de Entrada
```csharp
public async Task<Result> ProcessAsync(ProcessRequest request)
{
    if (request == null)
        throw new ArgumentNullException(nameof(request));
    
    var validationResult = await ValidateAsync(request);
    if (!validationResult.IsValid)
        return Result.Failure(validationResult.Errors);
    
    // Processamento...
}
```

## 📈 Performance

### Async/Await
- Sempre use async/await para operações I/O
- Passe CancellationToken em métodos async
- Use ConfigureAwait(false) em bibliotecas

### Caching
- Cache dados que mudam pouco (instrumentos, configurações)
- Use IMemoryCache para cache local
- Redis para cache distribuído quando necessário

## 🎨 Copilot - Diretrizes Específicas

### Ao Sugerir Código:
1. **Sempre** considere o contexto de DDD e os bounded contexts
2. **Use** .NET Aspire para orquestração de infraestrutura
3. **Organize** por módulos seguindo contextos delimitados (Auth, Signals, DataManagement, Strategies)
4. **Centralize** configurações no módulo **Shared** para unificar tudo
5. **Use** os padrões estabelecidos (Repository, Domain Events)
6. **Inclua** logging e tratamento de erro apropriados
7. **Considere** performance e async patterns
8. **Valide** entrada de dados
9. **Use** os tipos específicos do domínio (Price, Volume, Symbol)
10. **Publique** eventos de domínio quando apropriado
11. **Mantenha** separação entre contextos/módulos
12. **Use** dependency injection modular no Infrastructure
13. **Inclua** documentação XML para APIs públicas
14. **Configure** pipelines e middlewares por módulo
15. **Mantenha** projetos de API/Web mínimos, delegando para Infrastructure.Shared

### Ao Gerar Testes:
1. **Teste** cenários positivos e negativos
2. **Mock** dependências externas
3. **Use** dados realistas para trading
4. **Teste** validações de negócio
5. **Verifique** eventos publicados

### Arquivos de Teste HTTP:
1. **Localização**: Arquivos `.http` devem ficar no mesmo diretório do controller que testam
2. **Nomenclatura**: Use o mesmo nome do controller (ex: `AuthController.http` para `AuthController.cs`)
3. **Organização**: Agrupe testes por funcionalidade com comentários descritivos
4. **Variáveis**: Use variáveis para URLs base e tokens para facilitar testes
5. **Exemplo**: `BotSinais.Infrastructure/Modules/Auth/Controllers/AuthController.http`

### Linguagem:
- **Comentários** em português brasileiro
- **Nomes** de classes/métodos em inglês
- **Documentação** em português
- **Logs** em português