using BotSinais.Domain.Modules.Signals.Entities;

namespace BotSinais.Domain.Modules.Signals.Interfaces;

/// <summary>
/// Interface para repositório de carteiras
/// </summary>
public interface IPortfolioRepository
{
    Task<Portfolio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Portfolio?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Portfolio>> GetByOwnerAsync(string owner, CancellationToken cancellationToken = default);
    Task<Portfolio> CreateAsync(Portfolio portfolio, CancellationToken cancellationToken = default);
    Task<Portfolio> UpdateAsync(Portfolio portfolio, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


