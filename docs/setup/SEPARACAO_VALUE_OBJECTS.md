# 📁 Separação de Value Objects - Padrão Arquitetural

## 🎯 Mudança Implementada

A partir de **30 de julho de 2025**, todos os **Value Objects** no projeto Bot Sinais devem estar em **arquivos individuais** seguindo o princípio da **responsabilidade única** e facilitando a **manutenibilidade**.

## ✅ Antes vs Depois

### ❌ **Antes** - Arquivos Agrupados
```
ValueObjects/
└── RpaValueObjects.cs          # ❌ Múltiplos Value Objects em um arquivo
    ├── MongoConfiguration
    ├── ApiConfiguration  
    ├── DataQualityScore
    ├── MongoDataReference
    ├── ProcessingPriority
    └── RpaProcessingStatus
```

### ✅ **Depois** - Arquivos Individuais
```
ValueObjects/
├── MongoConfiguration.cs       # ✅ Um Value Object por arquivo
├── ApiConfiguration.cs         # ✅ Um Value Object por arquivo  
├── DataQualityScore.cs          # ✅ Um Value Object por arquivo
├── MongoDataReference.cs        # ✅ Um Value Object por arquivo
├── ProcessingPriority.cs        # ✅ Um Value Object por arquivo
└── RpaProcessingStatus.cs       # ✅ Um Value Object por arquivo
```

## 🔧 Padrão de Implementação

### **Estrutura de Arquivo de Value Object**
```csharp
// Arquivo: MongoConfiguration.cs
using BotSinais.Domain.Shared.ValueObjects;

namespace BotSinais.Domain.Modules.DataWarehouse.ValueObjects;

/// <summary>
/// Representa uma configuração MongoDB para RPAs
/// </summary>
public record MongoConfiguration
{
    public string DatabaseName { get; init; } = null!;
    public string CollectionTemplate { get; init; } = null!;
    // ... outras propriedades

    public static MongoConfiguration Create(
        string databaseName, 
        string collectionTemplate,
        string connectionString,
        int batchSize = 1000)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(databaseName))
            throw new ArgumentException("Database name cannot be empty", nameof(databaseName));
        
        // ... outras validações

        return new MongoConfiguration
        {
            DatabaseName = databaseName,
            CollectionTemplate = collectionTemplate,
            ConnectionString = connectionString,
            BatchSize = batchSize
        };
    }
}
```

## 📋 Regras de Nomenclatura

### **✅ Padrão Correto**
- **Nome do Arquivo**: `{ValueObjectName}.cs`
- **Nome da Classe**: Igual ao nome do arquivo
- **Namespace**: Seguir a estrutura de contextos delimitados

### **🎯 Exemplos por Contexto**

#### **DataManagement (Data Warehouse)**
```
BotSinais.Domain.Modules.DataWarehouse.ValueObjects/
├── MongoConfiguration.cs
├── ApiConfiguration.cs
├── DataQualityScore.cs
├── MongoDataReference.cs
├── ProcessingPriority.cs
└── RpaProcessingStatus.cs
```

#### **Signals (Trading)**
```
BotSinais.Domain.Modules.Signals.ValueObjects/
├── TradeQuantity.cs
├── RiskParameters.cs
├── PortfolioMetrics.cs
└── SignalStrength.cs
```

#### **Strategies (Execução)**
```
BotSinais.Domain.Modules.Strategies.ValueObjects/
├── BacktestSettings.cs
├── PerformanceMetrics.cs
├── OptimizationParameters.cs
└── ExecutionConfiguration.cs
```

## 🎨 Benefícios da Separação

### **1. Responsabilidade Única**
- Cada arquivo contém apenas um conceito de domínio
- Facilita a compreensão e manutenção
- Reduz acoplamento entre conceitos

### **2. Navegação Melhorada**
- Busca por arquivo específico mais fácil
- Intellisense mais preciso
- Menos conflitos de merge no Git

### **3. Testabilidade**
- Testes mais focados e específicos
- Mocking mais granular quando necessário
- Cobertura de código mais precisa

### **4. Refatoração Segura**
- Mudanças isoladas por conceito
- Menos risco de quebrar outros Value Objects
- Evolução independente dos conceitos

## 🚨 Diretrizes Obrigatórias

### **📁 Estrutura de Arquivo**
```csharp
// SEMPRE incluir namespace correto
namespace BotSinais.Domain.Modules.{Context}.ValueObjects;

// SEMPRE documentar com XML
/// <summary>
/// Descrição clara do propósito do Value Object
/// </summary>
public record {ValueObjectName}
{
    // Propriedades com validação
    public string RequiredProperty { get; init; } = null!;
    
    // Método de criação com validações
    public static {ValueObjectName} Create(params...)
    {
        // Validações obrigatórias
        // Retorno do objeto validado
    }
}
```

### **🔒 Validações Obrigatórias**
- **Nullability**: Propriedades requeridas marcadas com `= null!`
- **Business Rules**: Validações de negócio no método `Create`
- **Immutability**: Usar `record` e propriedades `init`
- **Documentation**: XML documentation para todas as propriedades públicas

### **📝 Convenções de Nomeação**
- **PascalCase** para nomes de arquivos: `MongoConfiguration.cs`
- **PascalCase** para propriedades: `DatabaseName`
- **camelCase** para parâmetros: `databaseName`
- **Descriptive Names**: Nomes que expressem claramente o conceito

## 🔄 Migração de Código Existente

### **Passo 1: Identificar Value Objects Agrupados**
```bash
# Buscar por arquivos com múltiplos records
grep -r "public record.*{" --include="*.cs" | grep -c "public record" | sort
```

### **Passo 2: Extrair para Arquivos Individuais**
1. Copiar cada `record` para arquivo próprio
2. Ajustar imports necessários
3. Verificar compilação
4. Remover arquivo original
5. Atualizar imports nas dependências

### **Passo 3: Validar Estrutura**
```bash
# Verificar se cada arquivo contém apenas um record
find . -name "*.cs" -exec grep -l "public record" {} \; | xargs -I {} bash -c 'echo "File: {}"; grep -c "public record" {}'
```

## 🏗️ Impacto na Arquitetura

### **✅ Domain Layer**
- Conceitos mais organizados e focados
- Facilita aplicação dos padrões DDD
- Melhora a expressividade do domínio

### **✅ Infrastructure Layer**
- Imports mais específicos e claros
- Menos dependências desnecessárias
- Configuração mais granular

### **✅ Application Layer**
- DTOs mapeados de forma mais precisa
- Validações mais específicas por contexto
- Casos de uso mais expressivos

## 📊 Métricas de Qualidade

### **Antes da Separação**
- ❌ 1 arquivo com 6 Value Objects
- ❌ 300+ linhas por arquivo
- ❌ Responsabilidades múltiplas

### **Depois da Separação**
- ✅ 6 arquivos com 1 Value Object cada
- ✅ ~50 linhas por arquivo
- ✅ Responsabilidade única por arquivo

---

## 🎯 Conclusão

A separação de **Value Objects em arquivos individuais** é uma prática obrigatória que:

✅ **Melhora a organização** do código  
✅ **Facilita a manutenção** e evolução  
✅ **Aumenta a clareza** dos conceitos de domínio  
✅ **Reduz conflitos** de merge no controle de versão  
✅ **Segue as melhores práticas** de Domain-Driven Design  

**Todos os novos Value Objects devem seguir este padrão.**
