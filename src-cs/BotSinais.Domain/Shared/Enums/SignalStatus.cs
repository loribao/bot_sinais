namespace BotSinais.Domain.Shared.Enums;

/// <summary>
/// Enumeração para status de sinais
/// </summary>
public enum SignalStatus
{
    Generated,      // Gerado
    Active,         // Ativo
    Executed,       // Executado
    Cancelled,      // Cancelado
    Expired,        // Expirado
    Failed          // Falhou
}

