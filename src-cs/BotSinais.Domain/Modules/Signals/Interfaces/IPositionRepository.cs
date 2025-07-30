using BotSinais.Domain.Modules.Signals.Entities;

namespace BotSinais.Domain.Modules.Signals.Interfaces;

/// <summary>
/// Interface para repositório de posições
/// </summary>
public interface IPositionRepository
{
    Task<Position?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Position>> GetByPortfolioAsync(Guid portfolioId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Position>> GetOpenPositionsAsync(Guid portfolioId, CancellationToken cancellationToken = default);
    Task<Position?> GetPositionAsync(Guid portfolioId, Guid instrumentId, CancellationToken cancellationToken = default);
    Task<Position> CreateAsync(Position position, CancellationToken cancellationToken = default);
    Task<Position> UpdateAsync(Position position, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


