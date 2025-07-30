# Configuração de Autenticação Keycloak - Bot Sinais

## 📋 Visão Geral

Este documento descreve como configurar e usar a autenticação JWT com Keycloak no projeto Bot Sinais.

## 🛠️ Configuração do Keycloak

### 1. Executar Keycloak via Docker

```bash
docker run -p 8080:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:latest start-dev
```

### 2. Configurar Cliente no Keycloak

1. Acesse: http://localhost:8080/admin
2. Faça login com `admin/admin`
3. Vá para **Clients** → **Create client**
4. Configure:
   - **Client ID**: `bot-signal-api`
   - **Client authentication**: `ON`
   - **Authorization**: `ON`
   - **Standard flow**: `ON`
   - **Direct access grants**: `ON`

### 3. Obter Client Secret

1. Na aba **Credentials** do cliente
2. Copie o valor de **Client secret**
3. Atualize o arquivo `appsettings.Development.json`

## 🔧 Configuração da API

### Arquivos de Configuração

#### appsettings.Development.json
```json
{
  "Keycloak": {
    "realm": "master",
    "auth-server-url": "http://localhost:8080/",
    "ssl-required": "external",
    "resource": "bot-signal-api",
    "verify-token-audience": true,
    "credentials": {
      "secret": "seu-client-secret-aqui"
    },
    "confidential-port": 0
  }
}
```

## 🚀 Como Usar

### 1. Obter Token JWT

**Endpoint**: `POST http://localhost:8080/realms/master/protocol/openid-connect/token`

**Headers**:
```
Content-Type: application/x-www-form-urlencoded
```

**Body**:
```
grant_type=password
&client_id=bot-signal-api
&client_secret=seu-client-secret
&username=admin
&password=admin
```

**Resposta**:
```json
{
  "access_token": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expires_in": 300,
  "refresh_expires_in": 1800,
  "refresh_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "token_type": "Bearer"
}
```

### 2. Usar Token nas Requisições

**Headers**:
```
Authorization: Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📡 Endpoints da API

### 🔓 Endpoints Públicos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/auth/info` | Informações sobre autenticação |
| GET | `/api/tradingsignals/health` | Status da API |

### 🔒 Endpoints Protegidos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/auth/me` | Informações do usuário |
| GET | `/api/auth/validate` | Validar token JWT |
| GET | `/api/auth/claims` | Listar claims do usuário |
| GET | `/api/tradingsignals/profile` | Perfil do usuário |
| POST | `/api/tradingsignals/market-data` | Enviar dados de mercado |
| POST | `/api/tradingsignals/generate-signal` | Gerar sinal de trading |

## 🧪 Testando com HTTP Files

Use o arquivo `Controllers/Auth.http` para testar todos os endpoints:

1. Execute a sequência de testes no arquivo
2. O token será obtido automaticamente e usado nos testes subsequentes
3. Teste cenários de sucesso e erro

## 🔍 Exemplo de Teste Manual

### 1. Obter Token
```bash
curl -X POST "http://localhost:8080/realms/master/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password&client_id=bot-signal-api&client_secret=SEU_SECRET&username=admin&password=admin"
```

### 2. Testar Endpoint Protegido
```bash
curl -X GET "https://localhost:7001/api/auth/me" \
  -H "Authorization: Bearer SEU_ACCESS_TOKEN"
```

## 🛡️ Recursos de Segurança

### Middleware de Tratamento de Erros
- Respostas padronizadas para erros 401/403
- Logs detalhados de tentativas de acesso
- Informações úteis para debugging

### Validação de Token
- Verificação automática da assinatura JWT
- Validação de expiração
- Verificação de audience e issuer

### Claims Disponíveis
- `sub`: ID do usuário
- `preferred_username`: Nome de usuário
- `email`: Email do usuário
- `given_name`: Primeiro nome
- `family_name`: Sobrenome
- `realm_access.roles`: Roles do usuário

## 🚨 Troubleshooting

### Problema: Token Inválido
**Solução**: Verifique se o client secret está correto e se o token não expirou.

### Problema: 401 Unauthorized
**Solução**: Certifique-se de incluir o header `Authorization: Bearer {token}`.

### Problema: 403 Forbidden
**Solução**: Verifique se o usuário possui as permissões necessárias no Keycloak.

### Problema: Keycloak não conecta
**Solução**: Verifique se o Keycloak está rodando em `http://localhost:8080`.

## 🔄 Fluxo de Autenticação

```mermaid
sequenceDiagram
    participant Client
    participant API
    participant Keycloak
    
    Client->>Keycloak: POST /token (credenciais)
    Keycloak->>Client: access_token JWT
    Client->>API: GET /api/protected (Bearer token)
    API->>Keycloak: Validar token
    Keycloak->>API: Token válido + claims
    API->>Client: Resposta autorizada
```

## 📝 Próximos Passos

1. **Implementar Roles**: Configurar diferentes níveis de acesso
2. **Refresh Token**: Implementar renovação automática de tokens
3. **Rate Limiting**: Adicionar limitação de requisições por usuário
4. **Audit Log**: Implementar log de auditoria de acessos
5. **CORS**: Configurar CORS para frontend
