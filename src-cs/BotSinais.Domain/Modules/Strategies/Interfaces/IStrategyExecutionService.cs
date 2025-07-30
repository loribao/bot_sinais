using BotSinais.Domain.Modules.Strategies.Entities;

namespace BotSinais.Domain.Modules.Strategies.Interfaces;

/// <summary>
/// Interface para serviço de execução de estratégias
/// </summary>
public interface IStrategyExecutionService
{
    Task<StrategyExecution> StartExecutionAsync(
        Guid strategyId, 
        Dictionary<string, object> parameters, 
        CancellationToken cancellationToken = default);
    
    Task<bool> StopExecutionAsync(Guid executionId, CancellationToken cancellationToken = default);
    Task<bool> PauseExecutionAsync(Guid executionId, CancellationToken cancellationToken = default);
    Task<bool> ResumeExecutionAsync(Guid executionId, CancellationToken cancellationToken = default);
    
    Task<string> GetExecutionStatusAsync(Guid executionId, CancellationToken cancellationToken = default);
    Task<string> GetExecutionLogAsync(Guid executionId, CancellationToken cancellationToken = default);
    
    Task<bool> ValidateStrategyAsync(Strategy strategy, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetValidationErrorsAsync(Strategy strategy, CancellationToken cancellationToken = default);
}


