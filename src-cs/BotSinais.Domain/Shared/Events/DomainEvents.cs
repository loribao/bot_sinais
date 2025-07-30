namespace BotSinais.Domain.Shared.Events;

// ===============================
// EVENTOS ORGANIZADOS POR CONTEXTO
// ===============================

/*
 * Os eventos estão organizados em pastas dentro de Events/:
 * 
 * - DataManagement/:
 *   - MarketDataReceivedEvent.cs
 *   - MarketDataUpdatedEvent.cs
 *   - InstrumentAddedEvent.cs
 *   - HistoricalDataLoadedEvent.cs
 * 
 * - Signals/:
 *   - SignalGeneratedEvent.cs
 *   - SignalExecutedEvent.cs
 *   - SignalStatusChangedEvent.cs
 *   - PositionOpenedEvent.cs
 *   - PositionClosedEvent.cs
 * 
 * - Strategies/:
 *   - StrategyCreatedEvent.cs
 *   - StrategyExecutionStartedEvent.cs
 *   - StrategyExecutionCompletedEvent.cs
 *   - BacktestCompletedEvent.cs
 * 
 * - System/:
 *   - SystemErrorEvent.cs
 *   - ExecuteStrategyRequestEvent.cs
 *   - DataAnalysisRequestEvent.cs
 *
 * Para usar os eventos, importe diretamente o arquivo específico:
 * using BotSinais.Domain.Shared.Events.DataManagement;
 * using BotSinais.Domain.Shared.Events.Signals;
 * using BotSinais.Domain.Shared.Events.Strategies;
 * using BotSinais.Domain.Shared.Events.System;
 *
 * As interfaces base (IDomainEvent e DomainEvent) estão em DomainEventBase.cs
 */


