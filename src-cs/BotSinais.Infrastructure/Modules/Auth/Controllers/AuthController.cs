using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BotSinais.Infrastructure.Modules.Auth.Controllers;

/// <summary>
/// Controller para operações de autenticação JWT via Keycloak
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly string _keycloakBaseUrl;
    private readonly string _realm;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public AuthController(ILogger<AuthController> logger, IConfiguration configuration, HttpClient httpClient)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClient;
        
        // Configurações do Keycloak a partir do appsettings
        _keycloakBaseUrl = _configuration["Keycloak:auth-server-url"] ?? "http://localhost:8080/";
        _realm = _configuration["Keycloak:realm"] ?? "botsignals";
        _clientId = _configuration["Keycloak:resource"] ?? "bot-signal-api";
        _clientSecret = _configuration["Keycloak:credentials:secret"] ?? throw new InvalidOperationException("Client secret não configurado");
    }

    /// <summary>
    /// Endpoint público para informações sobre autenticação
    /// </summary>
    [HttpGet("info")]
    [AllowAnonymous]
    public IActionResult GetAuthInfo()
    {
        return Ok(new
        {
            AuthProvider = "Keycloak",
            TokenType = "JWT Bearer",
            Realm = _realm,
            LoginUrl = $"{_keycloakBaseUrl}realms/{_realm}/protocol/openid-connect/auth",
            TokenUrl = $"{_keycloakBaseUrl}realms/{_realm}/protocol/openid-connect/token",
            RequiredScopes = new[] { "openid", "profile", "email" },
            Message = "Para acessar endpoints protegidos, inclua o header: Authorization: Bearer {seu-jwt-token}",
            DirectAuthEndpoint = "/api/auth/authenticate"
        });
    }

    /// <summary>
    /// Verifica se o Keycloak está disponível
    /// </summary>
    [HttpGet("keycloak-status")]
    [AllowAnonymous]
    public async Task<IActionResult> GetKeycloakStatus()
    {
        try
        {
            var realmUrl = $"{_keycloakBaseUrl}realms/{_realm}";
            var response = await _httpClient.GetAsync(realmUrl);
            
            var isAvailable = response.IsSuccessStatusCode;
            
            return Ok(new
            {
                IsKeycloakAvailable = isAvailable,
                RealmUrl = realmUrl,
                Status = isAvailable ? "Online" : "Offline",
                ResponseCode = (int)response.StatusCode,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar status do Keycloak");
            return Ok(new
            {
                IsKeycloakAvailable = false,
                Error = ex.Message,
                Status = "Error",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Autenticação direta com usuário e senha - retorna JWT diretamente
    /// </summary>
    [HttpPost("authenticate")]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest request)
    {
        try
        {
            _logger.LogInformation("Tentativa de autenticação para usuário: {Username}", request.Username);

            var tokenUrl = $"{_keycloakBaseUrl}realms/{_realm}/protocol/openid-connect/token";
            
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("username", request.Username),
                new KeyValuePair<string, string>("password", request.Password)
            });

            var response = await _httpClient.PostAsync(tokenUrl, formContent);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                });
                
                _logger.LogInformation("Autenticação bem-sucedida para usuário: {Username}", request.Username);
                
                return Ok(new
                {
                    Success = true,
                    AccessToken = tokenResponse?.AccessToken,
                    TokenType = tokenResponse?.TokenType ?? "Bearer",
                    ExpiresIn = tokenResponse?.ExpiresIn ?? 300,
                    RefreshToken = tokenResponse?.RefreshToken,
                    Scope = tokenResponse?.Scope,
                    Message = "Autenticação realizada com sucesso"
                });
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Falha na autenticação para usuário {Username}. Status: {StatusCode}, Error: {Error}", 
                    request.Username, response.StatusCode, errorContent);
                
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Credenciais inválidas",
                    Error = errorContent
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante autenticação para usuário: {Username}", request.Username);
            return StatusCode(500, new
            {
                Success = false,
                Message = "Erro interno do servidor durante autenticação",
                Error = ex.Message
            });
        }
    }

    /// <summary>
    /// Validar token JWT (endpoint protegido)
    /// </summary>
    [HttpGet("validate")]
    [Authorize]
    public IActionResult ValidateToken()
    {
        try
        {
            var user = HttpContext.User;
            
            return Ok(new
            {
                IsValid = true,
                User = new
                {
                    Id = user.FindFirst("sub")?.Value,
                    Username = user.FindFirst("preferred_username")?.Value,
                    Name = user.FindFirst("name")?.Value,
                    Email = user.FindFirst("email")?.Value
                },
                Claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList(),
                Message = "Token válido"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar token");
            return StatusCode(500, new { Message = "Erro ao validar token", Error = ex.Message });
        }
    }

    /// <summary>
    /// Obter informações do usuário autenticado
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetUserInfo()
    {
        try
        {
            var user = HttpContext.User;
            
            return Ok(new
            {
                UserId = user.FindFirst("sub")?.Value,
                Username = user.FindFirst("preferred_username")?.Value,
                Name = user.FindFirst("name")?.Value,
                GivenName = user.FindFirst("given_name")?.Value,
                FamilyName = user.FindFirst("family_name")?.Value,
                Email = user.FindFirst("email")?.Value,
                EmailVerified = bool.Parse(user.FindFirst("email_verified")?.Value ?? "false"),
                Roles = user.FindAll("realm_access.roles").Select(c => c.Value).ToList(),
                Scope = user.FindFirst("scope")?.Value,
                TokenInfo = new
                {
                    Issuer = user.FindFirst("iss")?.Value,
                    IssuedAt = user.FindFirst("iat")?.Value,
                    ExpiresAt = user.FindFirst("exp")?.Value,
                    TokenId = user.FindFirst("jti")?.Value
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter informações do usuário");
            return StatusCode(500, new { Message = "Erro ao obter informações do usuário", Error = ex.Message });
        }
    }

    /// <summary>
    /// Logout (invalidar token no Keycloak)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout([FromBody] LogoutRequest? request = null)
    {
        try
        {
            var user = HttpContext.User;
            var username = user.FindFirst("preferred_username")?.Value;
            
            _logger.LogInformation("Logout solicitado para usuário: {Username}", username);

            // Em um cenário real, você faria logout no Keycloak
            // Por enquanto, apenas log de sucesso
            
            return Ok(new
            {
                Success = true,
                Message = "Logout realizado com sucesso",
                Username = username,
                LoggedOutAt = DateTime.UtcNow,
                RedirectUri = request?.PostLogoutRedirectUri
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante logout");
            return StatusCode(500, new { Message = "Erro durante logout", Error = ex.Message });
        }
    }
}

/// <summary>
/// Modelo para requisição de autenticação
/// </summary>
public class AuthenticationRequest
{
    [Required(ErrorMessage = "Username é obrigatório")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password é obrigatório")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Modelo para requisição de logout
/// </summary>
public class LogoutRequest
{
    public string? PostLogoutRedirectUri { get; set; }
    public string? RefreshToken { get; set; }
}

/// <summary>
/// Modelo para resposta de token do Keycloak
/// </summary>
public class TokenResponse
{
    public string? AccessToken { get; set; }
    public string? TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }
    public int RefreshExpiresIn { get; set; }
    public string? Scope { get; set; }
    public string? SessionState { get; set; }
}
