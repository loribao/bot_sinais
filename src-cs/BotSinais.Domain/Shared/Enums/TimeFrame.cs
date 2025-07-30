namespace BotSinais.Domain.Shared.Enums;

/// <summary>
/// Enumeração para tipos de timeframes
/// </summary>
public enum TimeFrame
{
    M1 = 1,         // 1 minuto
    M5 = 5,         // 5 minutos
    M15 = 15,       // 15 minutos
    M30 = 30,       // 30 minutos
    H1 = 60,        // 1 hora
    H4 = 240,       // 4 horas
    D1 = 1440,      // 1 dia
    W1 = 10080,     // 1 semana
    MN1 = 43200     // 1 mês
}

