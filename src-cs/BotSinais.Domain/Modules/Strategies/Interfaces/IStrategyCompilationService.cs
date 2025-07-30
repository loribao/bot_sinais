using BotSinais.Domain.Shared;
using BotSinais.Domain.Shared.Enums;
using BotSinais.Domain.Shared.ValueObjects;
using BotSinais.Domain.Modules.Strategies.Entities;

namespace BotSinais.Domain.Modules.Strategies.Interfaces;

/// <summary>
/// Interface para serviço de compilação de estratégias
/// </summary>
public interface IStrategyCompilationService
{
    Task<bool> CompileStrategyAsync(Strategy strategy, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetCompilationErrorsAsync(Strategy strategy, CancellationToken cancellationToken = default);
    Task<bool> ValidateSyntaxAsync(string sourceCode, ExecutionLanguage language, CancellationToken cancellationToken = default);
    
    Task<string> GetCompiledPathAsync(Guid strategyId, CancellationToken cancellationToken = default);
    Task CleanCompiledFilesAsync(Guid strategyId, CancellationToken cancellationToken = default);
}



