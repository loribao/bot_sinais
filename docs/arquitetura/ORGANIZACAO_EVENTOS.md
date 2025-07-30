# 📝 Organização de Eventos - Bot Sinais

## 🎯 **Padrão de Arquivos Separados**

Seguindo as **diretrizes do copilot-instructions.md**, todos os eventos devem estar em **arquivos separados** para facilitar a manutenção, navegação e compreensão do código.

## 📂 **Estrutura de Diretórios**

```
BotSinais.Domain/
├── Modules/
│   ├── DataWarehouse/
│   │   └── Events/
│   │       ├── StartDataCollectionCommand.cs
│   │       ├── StopDataCollectionCommand.cs
│   │       ├── ConfigureDataCollectionCommand.cs
│   │       ├── DataCollectionStatusEvent.cs
│   │       ├── DataAvailableEvent.cs
│   │       └── RpaErrorEvent.cs
│   ├── Signals/
│   │   └── Events/
│   │       ├── SignalGeneratedEvent.cs
│   │       ├── SignalExecutedEvent.cs
│   │       └── SignalCancelledEvent.cs
│   ├── DataManagement/
│   │   └── Events/
│   │       ├── MarketDataReceivedEvent.cs
│   │       ├── InstrumentAddedEvent.cs
│   │       └── PriceUpdatedEvent.cs
│   └── Strategies/
│       └── Events/
│           ├── StrategyExecutedEvent.cs
│           ├── BacktestCompletedEvent.cs
│           └── StrategyOptimizedEvent.cs
└── Shared/
    └── Events/
        ├── DomainEventBase.cs
        ├── IDomainEventPublisher.cs
        └── System/
            ├── SystemErrorEvent.cs
            └── SystemStartedEvent.cs
```

## ✅ **Padrão de Implementação**

### **1. Estrutura Básica**
```csharp
using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.ValueObjects; // se necessário
using BotSinais.Domain.Shared.Enums; // se necessário

namespace BotSinais.Domain.Modules.{ModuleName}.Events;

/// <summary>
/// Descrição clara do que o evento representa
/// </summary>
public record {EventName} : DomainEvent
{
    public override string EventType => "{EventName}";
    
    // Propriedades específicas do evento
    public Guid SomeId { get; init; }
    public string SomeProperty { get; init; } = null!;
    // ...
}
```

### **2. Convenções de Nomenclatura**

#### **Eventos de Domínio**
- **Sufixo**: `Event`
- **Tempo**: Passado (algo que aconteceu)
- **Exemplos**: 
  - `SignalGeneratedEvent` ✅
  - `MarketDataReceivedEvent` ✅
  - `TradeExecutedEvent` ✅

#### **Comandos**
- **Sufixo**: `Command`
- **Tempo**: Imperativo (algo a ser feito)
- **Exemplos**:
  - `StartDataCollectionCommand` ✅
  - `ExecuteTradeCommand` ✅
  - `CancelOrderCommand` ✅

#### **Queries**
- **Sufixo**: `Query`
- **Tempo**: Presente (pergunta/consulta)
- **Exemplos**:
  - `GetActiveSignalsQuery` ✅
  - `FindInstrumentQuery` ✅

### **3. Propriedades Obrigatórias**

```csharp
public record ExampleEvent : DomainEvent
{
    // ✅ OBRIGATÓRIO: Override EventType
    public override string EventType => "ExampleEvent";
    
    // ✅ SEMPRE: IDs como Guid
    public Guid EntityId { get; init; }
    
    // ✅ OPCIONAL: Dados específicos do evento
    public string? AdditionalInfo { get; init; }
    
    // ✅ COLLECTIONS: Use init para imutabilidade
    public Dictionary<string, object> Metadata { get; init; } = new();
}
```

## 🔄 **Exemplos Implementados**

### **DataWarehouse Events**

#### **StartDataCollectionCommand.cs**
```csharp
public record StartDataCollectionCommand : DomainEvent
{
    public override string EventType => "StartDataCollectionCommand";
    
    public Guid RequestId { get; init; }
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public Symbol? Symbol { get; init; }
    public TimeFrame? TimeFrame { get; init; }
    public Dictionary<string, object> Parameters { get; init; } = new();
    public int Priority { get; init; } = 5;
    public TimeSpan Timeout { get; init; } = TimeSpan.FromMinutes(30);
}
```

#### **DataAvailableEvent.cs**
```csharp
public record DataAvailableEvent : DomainEvent
{
    public override string EventType => "DataAvailableEvent";
    
    public Guid RequestId { get; init; }
    public string RpaType { get; init; } = null!;
    public string DataSource { get; init; } = null!;
    public string CollectionName { get; init; } = null!;
    public string DatabaseName { get; init; } = null!;
    public string DataType { get; init; } = null!;
    public Symbol? Symbol { get; init; }
    public TimeFrame? TimeFrame { get; init; }
    public DateTime DataTimestamp { get; init; }
    public int RecordCount { get; init; }
    public long DataSizeBytes { get; init; }
    public Dictionary<string, object> Schema { get; init; } = new();
    public string QualityScore { get; init; } = "Unknown";
}
```

## 📋 **Checklist para Novos Eventos**

### **Antes de Criar**
- [ ] Nome segue convenção (Event/Command/Query)
- [ ] Está no módulo correto
- [ ] Não existe evento similar

### **Durante a Criação**
- [ ] Arquivo próprio com nome do evento
- [ ] Herda de `DomainEvent`
- [ ] Implementa `EventType` corretamente
- [ ] Documentação XML clara
- [ ] Usa `record` para imutabilidade
- [ ] Propriedades `init` para readonly
- [ ] Imports apenas necessários

### **Após a Criação**
- [ ] Compilação sem erros
- [ ] Atualizar documentação se necessário
- [ ] Considerar handlers necessários
- [ ] Validar com equipe se evento é necessário

## 🚀 **Benefícios da Organização**

### **1. 📁 Navegação Facilitada**
- **Localização rápida** de eventos específicos
- **Autocompletar** do IDE funciona melhor
- **Busca por arquivos** mais eficiente

### **2. 🔧 Manutenção Simplificada**
- **Mudanças isoladas** em cada evento
- **Conflitos de merge** reduzidos
- **Review de código** mais focado

### **3. 📖 Compreensão Melhorada**
- **Eventos relacionados** agrupados por módulo
- **Responsabilidades claras** por arquivo
- **Documentação** próxima ao código

### **4. 🧪 Testes Facilitados**
- **Testes unitários** mais específicos
- **Mocks** mais simples
- **Cobertura de código** mais precisa

## 🎯 **Próximos Passos**

1. **Aplicar padrão** para eventos existentes em outros módulos
2. **Criar handlers** para eventos do DataWarehouse
3. **Implementar testes** para cada evento
4. **Documentar fluxos** de eventos entre módulos
5. **Configurar mensageria** para comunicação com RPAs

---

**📝 Nota**: Este padrão deve ser seguido consistentemente em todos os módulos do projeto para manter a organização e facilitar a manutenção do código.
