using BotSinais.Domain.Modules.TradingData.Entities;

namespace BotSinais.Domain.Modules.TradingData.Interfaces;

/// <summary>
/// Interface para repositório de fontes de dados
/// </summary>
public interface IDataSourceRepository
{
    Task<DataSource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DataSource?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<DataSource>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DataSource>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<DataSource> CreateAsync(DataSource dataSource, CancellationToken cancellationToken = default);
    Task<DataSource> UpdateAsync(DataSource dataSource, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


