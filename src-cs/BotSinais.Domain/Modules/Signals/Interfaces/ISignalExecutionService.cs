using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.Signals.Entities;

namespace BotSinais.Domain.Modules.Signals.Interfaces;

/// <summary>
/// Interface para serviço de execução de sinais
/// </summary>
public interface ISignalExecutionService
{
    Task<SignalExecution> ExecuteSignalAsync(Guid signalId, CancellationToken cancellationToken = default);
    Task<bool> CanExecuteSignalAsync(Guid signalId, CancellationToken cancellationToken = default);
    Task CancelSignalAsync(Guid signalId, string reason, CancellationToken cancellationToken = default);
    Task UpdateSignalStatusAsync(Guid signalId, SignalStatus newStatus, string? reason = null, CancellationToken cancellationToken = default);
}



