using BotSinais.Domain.Shared.Events;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using InfrastructureEventPublisher = BotSinais.Infrastructure.Shared.Events.IDomainEventPublisher;

namespace BotSinais.Infrastructure.Modules.Signals.Controllers;

/// <summary>
/// Controller para operações de sinais de trading
/// Versão temporária enquanto reorganizamos os eventos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Protege todo o controller com autenticação
public class TradingSignalsController : ControllerBase
{
    private readonly InfrastructureEventPublisher _eventPublisher;
    private readonly ILogger<TradingSignalsController> _logger;

    public TradingSignalsController(
        InfrastructureEventPublisher eventPublisher,
        ILogger<TradingSignalsController> logger)
    {
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint para receber dados de mercado
    /// </summary>
    [HttpPost("market-data")]
    public async Task<IActionResult> ReceiveMarketData([FromBody] MarketDataRequest request)
    {
        try
        {
            _logger.LogInformation("Recebendo dados de mercado para {Symbol}", request.Symbol);

            // TODO: Criar evento MarketDataReceivedEvent após reorganização
            // Por agora apenas logar a recepção dos dados
            
            return Ok(new { Message = "Dados de mercado recebidos com sucesso", Timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar dados de mercado");
            return StatusCode(500, new { Message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Endpoint para gerar sinais de trading
    /// </summary>
    [HttpPost("generate-signal")]
    public async Task<IActionResult> GenerateSignal([FromBody] GenerateSignalRequest request)
    {
        try
        {
            _logger.LogInformation("Gerando sinal para {InstrumentId}", request.InstrumentId);

            // TODO: Criar evento SignalGeneratedEvent após reorganização
            // Por agora apenas logar a geração do sinal
            
            return Ok(new 
            { 
                Message = "Sinal gerado com sucesso", 
                SignalId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Direction = request.Direction.ToString()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar sinal");
            return StatusCode(500, new { Message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Endpoint para testar autenticação - requer token JWT válido
    /// </summary>
    [HttpGet("profile")]
    public IActionResult GetUserProfile()
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            var userName = User.FindFirst("preferred_username")?.Value;
            var email = User.FindFirst("email")?.Value;
            var roles = User.FindAll("realm_access.roles").Select(c => c.Value);

            return Ok(new
            {
                UserId = userId,
                UserName = userName,
                Email = email,
                Roles = roles,
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter perfil do usuário");
            return StatusCode(500, new { Message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Endpoint público para verificar status da API
    /// </summary>
    [HttpGet("health")]
    [AllowAnonymous] // Permite acesso sem autenticação
    public IActionResult Health()
    {
        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0"
        });
    }
}

/// <summary>
/// Request para dados de mercado
/// </summary>
public class MarketDataRequest
{
    [Required]
    public Guid InstrumentId { get; set; }

    [Required]
    [StringLength(10)]
    public string Symbol { get; set; } = null!;

    [Required]
    [StringLength(10)]
    public string Exchange { get; set; } = null!;

    [Required]
    public TimeFrame TimeFrame { get; set; }

    [Required]
    public DateTimeOffset Timestamp { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Open { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal High { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Low { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Close { get; set; }

    [Required]
    [Range(0, long.MaxValue)]
    public long Volume { get; set; }
}

/// <summary>
/// Request para geração de sinais
/// </summary>
public class GenerateSignalRequest
{
    [Required]
    public Guid InstrumentId { get; set; }

    [Required]
    public Guid StrategyId { get; set; }

    [Required]
    public TradeDirection Direction { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal EntryPrice { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? StopLoss { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? TakeProfit { get; set; }

    [Range(0, 100)]
    public decimal Confidence { get; set; } = 50;
}
