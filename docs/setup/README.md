# ⚙️ Setup e Configuração - Bot Sinais

Esta pasta contém documentação sobre configuração, setup e organização do sistema Bot Sinais.

## 📋 **Documentos de Configuração**

### 🔧 **Configuração Geral**
- **[SETUP_CONFIGURACAO.md](./SETUP_CONFIGURACAO.md)**
  - Configuração inicial do sistema
  - Variáveis de ambiente
  - Dependências e pré-requisitos
  - Configuração do .NET Aspire

### 📨 **Mensageria**
- **[MASSTRANSIT_SETUP.md](./MASSTRANSIT_SETUP.md)**
  - Configuração do MassTransit
  - Setup do RabbitMQ
  - Configuração de consumers
  - Padrões de messaging

### � **Integração com RPAs**
- **[ESTRUTURA_RPA_INTEGRACAO.md](./ESTRUTURA_RPA_INTEGRACAO.md)** - 🔥 **Estrutura para integração com RPAs externos**
  - Protocolo de mensageria com RPAs
  - Estrutura MongoDB padrão
  - Especificações técnicas
  - Casos de uso e exemplos

### �🧪 **Testes HTTP**
- **[ORGANIZACAO_HTTP_FILES.md](./ORGANIZACAO_HTTP_FILES.md)**
  - Organização dos arquivos .http
  - Padronização de testes
  - Estrutura de variáveis
  - Melhores práticas

## 🚀 **Guia de Setup Rápido**

### **1. Pré-requisitos**
```bash
# .NET 9.0 SDK
winget install Microsoft.DotNet.SDK.9

# Docker Desktop
winget install Docker.DockerDesktop

# VS Code + Extensions
winget install Microsoft.VisualStudioCode
code --install-extension ms-dotnettools.csharp
code --install-extension humao.rest-client
```

### **2. Clonar e Configurar**
```bash
# Clonar repositório
git clone https://github.com/loribao/bot_sinais.git
cd bot_sinais

# Navegar para o projeto
cd src-cs
```

### **3. Executar Sistema**
```bash
# Executar com Aspire
cd BotSinais.AppHost
dotnet run
```

### **4. Verificar Serviços**
- **Aspire Dashboard**: https://localhost:17053
- **API**: https://localhost:17053/api
- **Keycloak**: http://localhost:8080

## 🐳 **Configuração Docker**

### **Serviços Gerenciados pelo Aspire**
- **PostgreSQL** - Banco de dados principal
- **RabbitMQ** - Message broker
- **Keycloak** - Servidor de autenticação

### **Portas Padrão**
```yaml
PostgreSQL: 5432
RabbitMQ: 5672 (AMQP), 15672 (Management)
Keycloak: 8080
API Service: 17053
Web Service: 17054
```

## 🔧 **Configurações de Desenvolvimento**

### **appsettings.Development.json**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Keycloak": {
    "Authority": "http://localhost:8080/realms/botsignals",
    "ClientId": "bot-signal-api",
    "ClientSecret": "6eAKHyl8vbXq5FFhWpMUFzyvAjCPDEk2"
  }
}
```

### **Variáveis de Ambiente**
```bash
# Desenvolvimento
ASPNETCORE_ENVIRONMENT=Development
DOTNET_ENVIRONMENT=Development

# Produção (exemplo)
ASPNETCORE_ENVIRONMENT=Production
DATABASE_CONNECTION_STRING="[connection string]"
KEYCLOAK_CLIENT_SECRET="[secret]"
```

## 📁 **Estrutura de Configuração**

### **Por Projeto**
```
BotSinais.AppHost/
├── appsettings.json          # Configurações base
├── appsettings.Development.json  # Dev overrides
└── AppHost.cs               # Orquestração Aspire

BotSinais.ApiService/
├── appsettings.json         # API settings
└── Program.cs              # Pipeline mínimo

BotSinais.Infrastructure/
└── Modules/
    └── [Module]/
        ├── ServiceCollectionExtensions.cs  # DI
        └── WebApplicationExtensions.cs     # Pipeline
```

### **Configuração Modular**
Cada módulo tem suas próprias configurações:
- **Auth**: Keycloak, JWT, sessões
- **Signals**: Trading algorithms, risk management
- **DataManagement**: Database, external APIs
- **Strategies**: Execution engines, languages

## 🔒 **Segurança e Secrets**

### **Desenvolvimento**
```bash
# User Secrets
dotnet user-secrets init
dotnet user-secrets set "Keycloak:ClientSecret" "your-secret"
```

### **Produção**
- **Azure Key Vault** - Para secrets
- **Environment Variables** - Para configurações
- **Docker Secrets** - Em containers

## 🧪 **Configuração de Testes**

### **Arquivos HTTP**
Cada controller tem seu arquivo de teste:
```
BotSinais.Infrastructure/Modules/
├── Auth/Controllers/AuthController.http
├── Signals/Controllers/TradingSignalsController.http
└── [outros módulos]/Controllers/[Controller].http
```

### **Variáveis Compartilhadas**
```http
# Em cada arquivo .http
@base_url = https://localhost:17053
@keycloak_url = http://localhost:8080
@realm = botsignals
```

## 📊 **Monitoramento e Observabilidade**

### **Aspire Dashboard**
- **Traces**: Rastreamento de requests
- **Metrics**: Métricas de performance
- **Logs**: Logs estruturados
- **Health**: Status dos serviços

### **Logs Estruturados**
```csharp
// Exemplo de log
logger.LogInformation("User {UserId} authenticated successfully", userId);
```

### **Health Checks**
- **Database**: Conectividade PostgreSQL
- **Message Broker**: Status RabbitMQ
- **External APIs**: Keycloak, market data

## 🔄 **Fluxo de Deploy**

### **Desenvolvimento**
1. Código → Git
2. Git → Build local
3. Aspire → Containers locais

### **Produção (Futuro)**
1. Git → CI/CD Pipeline
2. Build → Container Images
3. Deploy → Kubernetes/Azure

## 📚 **Links Úteis**

### **Documentação Oficial**
- [.NET Aspire](https://learn.microsoft.com/aspire)
- [MassTransit](https://masstransit.io/)
- [Keycloak](https://www.keycloak.org/documentation)

### **Ferramentas**
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/) - Para PostgreSQL
- [RabbitMQ Management](http://localhost:15672) - Interface web

---

**⚙️ Objetivo**: Facilitar setup, configuração e manutenção do ambiente de desenvolvimento.

**📅 Última atualização**: 30 de julho de 2025  
**🔧 Status**: Configuração completa e funcional
