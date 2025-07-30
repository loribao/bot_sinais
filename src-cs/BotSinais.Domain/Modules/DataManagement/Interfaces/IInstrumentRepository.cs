using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.DataManagement.Entities;

namespace BotSinais.Domain.Modules.DataManagement.Interfaces;

/// <summary>
/// Interface para repositório de instrumentos
/// </summary>
public interface IInstrumentRepository
{
    Task<Instrument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Instrument?> GetBySymbolAsync(Symbol symbol, CancellationToken cancellationToken = default);
    Task<IEnumerable<Instrument>> GetByTypeAsync(InstrumentType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Instrument>> GetActiveInstrumentsAsync(CancellationToken cancellationToken = default);
    Task<Instrument> CreateAsync(Instrument instrument, CancellationToken cancellationToken = default);
    Task<Instrument> UpdateAsync(Instrument instrument, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}



