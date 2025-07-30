using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.Strategies.Entities;
using System.Diagnostics;

namespace BotSinais.Domain.Modules.Strategies.Interfaces;

/// <summary>
/// Interface para serviço de engines de execução
/// </summary>
public interface IExecutionEngineService
{
    Task<ExecutionEngine?> GetEngineAsync(ExecutionLanguage language, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExecutionEngine>> GetAvailableEnginesAsync(CancellationToken cancellationToken = default);
    
    Task<bool> IsEngineAvailableAsync(ExecutionLanguage language, CancellationToken cancellationToken = default);
    Task<string> GetEngineVersionAsync(ExecutionLanguage language, CancellationToken cancellationToken = default);
    
    Task<Process> StartStrategyProcessAsync(
        Strategy strategy,
        Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default);
    
    Task<bool> StopStrategyProcessAsync(string processId, CancellationToken cancellationToken = default);
    Task<bool> IsProcessRunningAsync(string processId, CancellationToken cancellationToken = default);
}



