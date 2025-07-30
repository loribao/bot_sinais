# Configuração do .NET Aspire para Bot Sinais

Esta é a configuração do AppHost para orquestrar toda a infraestrutura do Bot Sinais usando .NET Aspire.

## 📋 Recursos Orquestrados

### Infraestrutura
- **PostgreSQL** - Banco de dados principal
- **RabbitMQ** - Message broker para eventos
- **Keycloak** - Servidor de autenticação/autorização
- **Redis** - Cache distribuído

### Aplicações
- **BotSinais.ApiService** - API REST para sinais de trading
- **BotSinais.Web** - Interface web Blazor

## 🔧 Configuração do AppHost

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Recursos de infraestrutura
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .AddDatabase("botsinais");

var rabbitmq = builder.AddRabbitMQ("messaging")
    .WithDataVolume()
    .WithManagementPlugin();

var redis = builder.AddRedis("cache")
    .WithDataVolume();

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithDataVolume();

// Projetos da aplicação
var apiService = builder.AddProject<Projects.BotSinais_ApiService>("apiservice")
    .WithReference(postgres)
    .WithReference(rabbitmq)
    .WithReference(keycloak)
    .WithExternalHttpEndpoints();

var webApp = builder.AddProject<Projects.BotSinais_Web>("webapp")
    .WithReference(apiService)
    .WithReference(redis)
    .WithReference(keycloak)
    .WithExternalHttpEndpoints();

builder.Build().Run();
```

## 🚀 Como Executar

1. **Execute o AppHost:**
   ```bash
   cd src-cs
   dotnet run --project BotSinais.AppHost
   ```

2. **Acesse o Dashboard Aspire:**
   - URL: http://localhost:15888
   - Monitore logs, métricas e traces

3. **Recursos Disponíveis:**
   - API: https://localhost:7001
   - Web App: https://localhost:7002
   - Keycloak: http://localhost:8080
   - RabbitMQ Management: http://localhost:15672

## 🔐 Configuração de Autenticação

O Keycloak será automaticamente configurado com:
- Realm: `master`
- Admin User: `admin/admin`
- Client: `bot-signal-api`

## 📊 Observabilidade

O .NET Aspire fornece:
- **Logs centralizados** de todos os serviços
- **Métricas** de performance e saúde
- **Traces distribuídos** para debugging
- **Dashboard** unificado para monitoramento
