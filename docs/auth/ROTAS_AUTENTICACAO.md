# 🔐 Rotas de Autenticação - Bot Sinais

Este documento descreve as rotas de autenticação implementadas no sistema Bot Sinais usando integração com Keycloak.

## 📋 Endpoints Disponíveis

### 1. **GET /api/auth/info** (Público)
**Descrição:** Fornece informações sobre a configuração de autenticação  
**Autenticação:** Não requerida

**Resposta:**
```json
{
  "AuthProvider": "Keycloak",
  "TokenType": "JWT Bearer",
  "LoginUrl": "http://localhost:8080/realms/botsignals/protocol/openid-connect/auth",
  "TokenUrl": "http://localhost:8080/realms/botsignals/protocol/openid-connect/token",
  "RequiredScopes": ["openid", "profile", "email"],
  "Message": "Para acessar endpoints protegidos, inclua o header: Authorization: Bearer {seu-jwt-token}"
}
```

### 2. **GET /api/auth/login** (Público)
**Descrição:** Inicia o processo de login redirecionando para o Keycloak  
**Autenticação:** Não requerida  
**Parâmetros Query:**
- `redirectUri` (opcional): URL para redirecionamento após login

**Comportamento:**
- Gera um estado CSRF para segurança
- Armazena estado na sessão
- Redireciona para página de login do Keycloak

### 3. **GET /api/auth/callback** (Público)
**Descrição:** Callback executado após login no Keycloak  
**Autenticação:** Não requerida  
**Parâmetros Query:**
- `code`: Código de autorização retornado pelo Keycloak
- `state`: Estado CSRF para validação
- `error` (opcional): Erro caso login falhe

**Resposta de Sucesso:**
```json
{
  "Message": "Login realizado com sucesso",
  "Code": "authorization-code-here",
  "Instructions": "Use este código para trocar por um token JWT no endpoint /api/auth/token",
  "NextStep": "POST /api/auth/token com o código recebido"
}
```

### 4. **POST /api/auth/token** (Público)
**Descrição:** Troca código de autorização por token JWT  
**Autenticação:** Não requerida  
**Content-Type:** `application/json`

**Payload:**
```json
{
  "code": "authorization-code-from-callback",
  "redirectUri": "http://localhost:5274/api/auth/callback"
}
```

**Resposta:**
```json
{
  "Message": "Token obtido com sucesso",
  "TokenResponse": "{ access_token, refresh_token, etc. }",
  "ContentType": "application/json"
}
```

### 5. **GET /api/auth/me** (Protegido)
**Descrição:** Obtém informações do usuário autenticado  
**Autenticação:** JWT Bearer token obrigatório  
**Header:** `Authorization: Bearer {jwt-token}`

**Resposta:**
```json
{
  "Id": "user-id",
  "UserName": "username",
  "Email": "user@example.com",
  "FullName": "Nome Completo",
  "Roles": ["role1", "role2"],
  "IsAuthenticated": true,
  "AuthenticationType": "Bearer"
}
```

### 6. **GET /api/auth/validate** (Protegido)
**Descrição:** Valida se o token JWT ainda está válido  
**Autenticação:** JWT Bearer token obrigatório

**Resposta:**
```json
{
  "IsValid": true,
  "ExpiresAt": "2025-07-30T15:30:00Z",
  "Issuer": "http://localhost:8080/realms/botsignals",
  "Audience": "bot-signal-api",
  "IsExpired": false,
  "TimeToExpiry": "01:25:30"
}
```

### 7. **GET /api/auth/claims** (Protegido)
**Descrição:** Lista todas as claims do token JWT (útil para debug)  
**Autenticação:** JWT Bearer token obrigatório

**Resposta:**
```json
{
  "TotalClaims": 15,
  "Claims": [
    {
      "Type": "sub",
      "Value": "user-id",
      "ValueType": "string",
      "Issuer": "http://localhost:8080/realms/botsignals"
    }
  ]
}
```

### 8. **POST /api/auth/logout** (Protegido)
**Descrição:** Realiza logout do usuário  
**Autenticação:** JWT Bearer token obrigatório  
**Content-Type:** `application/json`

**Payload (opcional):**
```json
{
  "postLogoutRedirectUri": "http://localhost:3000/",
  "refreshToken": "refresh-token-if-available"
}
```

**Resposta:**
```json
{
  "Message": "Logout realizado com sucesso",
  "LogoutUrl": "http://localhost:8080/realms/botsignals/protocol/openid-connect/logout?...",
  "Instructions": "Redirecione para LogoutUrl para completar o logout no Keycloak"
}
```

### 9. **GET /api/auth/logged-out** (Público)
**Descrição:** Confirma que o logout foi realizado  
**Autenticação:** Não requerida

**Resposta:**
```json
{
  "Message": "Logout realizado com sucesso",
  "Status": "Desconectado",
  "Timestamp": "2025-07-30T14:05:00Z"
}
```

## 🔄 Fluxo Completo de Autenticação

### Processo de Login:
1. **Inicial:** `GET /api/auth/info` - Obter configurações
2. **Login:** `GET /api/auth/login` - Redirecionar para Keycloak
3. **Callback:** Keycloak retorna para `/api/auth/callback?code=xxx&state=yyy`
4. **Token:** `POST /api/auth/token` - Trocar código por JWT
5. **Uso:** Incluir token em requests: `Authorization: Bearer {token}`

### Processo de Logout:
1. **Logout:** `POST /api/auth/logout` - Limpar sessão local
2. **Keycloak:** Redirecionar para URL retornada para logout completo
3. **Confirmação:** Usuário é redirecionado para `/api/auth/logged-out`

## 🛡️ Segurança

### Proteções Implementadas:
- **CSRF Protection:** Estado único gerado para cada login
- **Validação de Estado:** Verificação do parâmetro state no callback
- **Sessões Seguras:** Cookies HttpOnly e SameSite
- **Timeout de Sessão:** 30 minutos de inatividade
- **Validação de Configuração:** Verificação de parâmetros Keycloak

### Headers de Segurança:
```http
Authorization: Bearer {jwt-token}
Content-Type: application/json
Accept: application/json
```

## ⚙️ Configuração Keycloak

As rotas utilizam as seguintes configurações do `appsettings.json`:

```json
{
  "Keycloak": {
    "realm": "botsignals",
    "auth-server-url": "http://localhost:8080/",
    "ssl-required": "external",
    "resource": "bot-signal-api",
    "credentials": {
      "secret": "client-secret-here"
    },
    "confidential-port": 0
  }
}
```

## 🧪 Testando as Rotas

Use o arquivo `test-auth-complete.http` para testar todas as rotas:

```bash
# Inicie a aplicação
cd BotSinais.AppHost
dotnet run

# Acesse o dashboard Aspire para ver status dos serviços
# https://localhost:17053

# Use o arquivo test-auth-complete.http no VS Code
# com a extensão REST Client
```

## 📝 Notas Importantes

1. **Sessões:** O sistema usa sessões em memória para armazenar estado OAuth
2. **HTTPS:** Em produção, configure HTTPS e ajuste `RequireHttpsMetadata=true`
3. **Tokens:** Tokens JWT devem ser armazenados de forma segura no cliente
4. **Refresh:** Implemente renovação automática usando refresh tokens
5. **Logs:** Todos os eventos de autenticação são logados para auditoria

## 🚀 Próximos Passos

- [ ] Implementar renovação automática de tokens
- [ ] Adicionar cache distribuído (Redis) para sessões
- [ ] Implementar rate limiting nos endpoints
- [ ] Adicionar middleware de auditoria
- [ ] Configurar HTTPS em produção
- [ ] Implementar roles e permissões granulares
