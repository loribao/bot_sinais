// Este arquivo contém importações globais para todos os eventos organizados
// Permite usar os eventos sem precisar especificar o namespace completo

global using MarketDataReceivedEvent = BotSinais.Domain.Shared.Events.DataManagement.MarketDataReceivedEvent;
global using MarketDataUpdatedEvent = BotSinais.Domain.Shared.Events.DataManagement.MarketDataUpdatedEvent;
global using InstrumentAddedEvent = BotSinais.Domain.Shared.Events.DataManagement.InstrumentAddedEvent;
global using HistoricalDataLoadedEvent = BotSinais.Domain.Shared.Events.DataManagement.HistoricalDataLoadedEvent;

global using SignalGeneratedEvent = BotSinais.Domain.Shared.Events.Signals.SignalGeneratedEvent;
global using SignalExecutedEvent = BotSinais.Domain.Shared.Events.Signals.SignalExecutedEvent;
global using SignalStatusChangedEvent = BotSinais.Domain.Shared.Events.Signals.SignalStatusChangedEvent;
global using PositionOpenedEvent = BotSinais.Domain.Shared.Events.Signals.PositionOpenedEvent;
global using PositionClosedEvent = BotSinais.Domain.Shared.Events.Signals.PositionClosedEvent;

global using StrategyCreatedEvent = BotSinais.Domain.Shared.Events.Strategies.StrategyCreatedEvent;
global using StrategyExecutionStartedEvent = BotSinais.Domain.Shared.Events.Strategies.StrategyExecutionStartedEvent;
global using StrategyExecutionCompletedEvent = BotSinais.Domain.Shared.Events.Strategies.StrategyExecutionCompletedEvent;
global using BacktestCompletedEvent = BotSinais.Domain.Shared.Events.Strategies.BacktestCompletedEvent;

global using SystemErrorEvent = BotSinais.Domain.Shared.Events.System.SystemErrorEvent;
global using ExecuteStrategyRequestEvent = BotSinais.Domain.Shared.Events.System.ExecuteStrategyRequestEvent;
global using DataAnalysisRequestEvent = BotSinais.Domain.Shared.Events.System.DataAnalysisRequestEvent;
