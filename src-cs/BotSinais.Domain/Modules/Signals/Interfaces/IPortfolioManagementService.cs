namespace BotSinais.Domain.Modules.Signals.Interfaces;

/// <summary>
/// Interface para serviço de gerenciamento de carteiras
/// </summary>
public interface IPortfolioManagementService
{
    Task<decimal> CalculatePositionSizeAsync(
        Guid portfolioId, 
        Guid signalId, 
        CancellationToken cancellationToken = default);
    
    Task<bool> CheckRiskLimitsAsync(
        Guid portfolioId, 
        Guid signalId, 
        decimal quantity, 
        CancellationToken cancellationToken = default);
    
    Task<decimal> CalculatePortfolioValueAsync(Guid portfolioId, CancellationToken cancellationToken = default);
    Task<decimal> CalculateUnrealizedPnLAsync(Guid portfolioId, CancellationToken cancellationToken = default);
    Task<decimal> CalculateDrawdownAsync(Guid portfolioId, CancellationToken cancellationToken = default);
    
    Task UpdatePositionsAsync(Guid portfolioId, CancellationToken cancellationToken = default);
}


