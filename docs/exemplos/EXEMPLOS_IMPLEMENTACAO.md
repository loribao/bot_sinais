# Exemplos Práticos - Engine de Estratégias Python

## 1. Exemplo de Classe Base para Estratégias

```python
from abc import ABC, abstractmethod
from typing import Dict, List, Optional
import pandas as pd
import numpy as np

class BaseStrategy(ABC):
    """Classe base para todas as estratégias de trading"""
    
    def __init__(self, name: str, timeframe: str, confidence_threshold: float = 0.7):
        self.name = name
        self.timeframe = timeframe
        self.confidence_threshold = confidence_threshold
        self.signals_history = []
    
    @abstractmethod
    def analyze(self, data: pd.DataFrame) -> Dict:
        """Analisa os dados e retorna sinal de trading"""
        pass
    
    @abstractmethod
    def get_entry_conditions(self) -> Dict:
        """Retorna as condições de entrada"""
        pass
    
    def calculate_confidence(self, indicators: Dict) -> float:
        """Calcula o nível de confiança do sinal"""
        pass
    
    def generate_signal(self, data: pd.DataFrame) -> Optional[Dict]:
        """Gera sinal de trading se as condições forem atendidas"""
        analysis = self.analyze(data)
        
        if analysis['confidence'] >= self.confidence_threshold:
            signal = {
                'strategy': self.name,
                'asset': analysis['asset'],
                'direction': analysis['direction'],
                'entry_price': analysis['entry_price'],
                'stop_loss': analysis['stop_loss'],
                'take_profit': analysis['take_profit'],
                'confidence': analysis['confidence'],
                'timestamp': pd.Timestamp.now(),
                'timeframe': self.timeframe,
                'indicators': analysis['indicators']
            }
            self.signals_history.append(signal)
            return signal
        
        return None
```

## 2. Exemplo de Estratégia de Reversão à Média

```python
class MeanReversionStrategy(BaseStrategy):
    """Estratégia de reversão à média com RSI e Bollinger Bands"""
    
    def __init__(self, rsi_period=14, bb_period=20, bb_std=2):
        super().__init__("Mean Reversion", "5m")
        self.rsi_period = rsi_period
        self.bb_period = bb_period
        self.bb_std = bb_std
    
    def calculate_rsi(self, prices: pd.Series, period: int = 14) -> pd.Series:
        """Calcula RSI"""
        delta = prices.diff()
        gain = (delta.where(delta > 0, 0)).rolling(window=period).mean()
        loss = (-delta.where(delta < 0, 0)).rolling(window=period).mean()
        rs = gain / loss
        rsi = 100 - (100 / (1 + rs))
        return rsi
    
    def calculate_bollinger_bands(self, prices: pd.Series, period: int = 20, std: int = 2):
        """Calcula Bollinger Bands"""
        sma = prices.rolling(window=period).mean()
        std_dev = prices.rolling(window=period).std()
        upper_band = sma + (std_dev * std)
        lower_band = sma - (std_dev * std)
        return upper_band, sma, lower_band
    
    def analyze(self, data: pd.DataFrame) -> Dict:
        """Analisa dados para sinais de reversão"""
        close_prices = data['close']
        current_price = close_prices.iloc[-1]
        
        # Calcula indicadores
        rsi = self.calculate_rsi(close_prices, self.rsi_period)
        upper_bb, middle_bb, lower_bb = self.calculate_bollinger_bands(
            close_prices, self.bb_period, self.bb_std
        )
        
        current_rsi = rsi.iloc[-1]
        current_upper_bb = upper_bb.iloc[-1]
        current_lower_bb = lower_bb.iloc[-1]
        current_middle_bb = middle_bb.iloc[-1]
        
        # Identifica condições
        oversold = current_rsi < 30
        overbought = current_rsi > 70
        price_below_lower_bb = current_price < current_lower_bb
        price_above_upper_bb = current_price > current_upper_bb
        
        # Determina direção e confiança
        direction = None
        confidence = 0
        
        if oversold and price_below_lower_bb:
            direction = "CALL"
            confidence = min(0.9, (30 - current_rsi) / 30 * 0.5 + 0.5)
        elif overbought and price_above_upper_bb:
            direction = "PUT"
            confidence = min(0.9, (current_rsi - 70) / 30 * 0.5 + 0.5)
        
        # Calcula pontos de entrada e saída
        if direction == "CALL":
            entry_price = current_price
            stop_loss = current_lower_bb * 0.999
            take_profit = current_middle_bb
        elif direction == "PUT":
            entry_price = current_price
            stop_loss = current_upper_bb * 1.001
            take_profit = current_middle_bb
        else:
            entry_price = stop_loss = take_profit = None
        
        return {
            'asset': data.attrs.get('symbol', 'UNKNOWN'),
            'direction': direction,
            'confidence': confidence,
            'entry_price': entry_price,
            'stop_loss': stop_loss,
            'take_profit': take_profit,
            'indicators': {
                'rsi': current_rsi,
                'bb_upper': current_upper_bb,
                'bb_middle': current_middle_bb,
                'bb_lower': current_lower_bb,
                'price_position': 'oversold' if oversold else 'overbought' if overbought else 'neutral'
            }
        }
    
    def get_entry_conditions(self) -> Dict:
        return {
            'CALL': {
                'RSI': '< 30 (oversold)',
                'Price': '< Lower Bollinger Band',
                'Confirmation': 'Volume above average'
            },
            'PUT': {
                'RSI': '> 70 (overbought)',
                'Price': '> Upper Bollinger Band',
                'Confirmation': 'Volume above average'
            }
        }
```

## 3. Exemplo de Coletor de Dados

```python
import asyncio
import aiohttp
import pandas as pd
from typing import Dict, List
import logging

class DataCollector:
    """Coleta dados de múltiplas fontes"""
    
    def __init__(self):
        self.session = None
        self.logger = logging.getLogger(__name__)
    
    async def __aenter__(self):
        self.session = aiohttp.ClientSession()
        return self
    
    async def __aexit__(self, exc_type, exc_val, exc_tb):
        if self.session:
            await self.session.close()
    
    async def get_binance_data(self, symbol: str, interval: str = "5m", limit: int = 100) -> pd.DataFrame:
        """Coleta dados da Binance"""
        url = "https://api.binance.com/api/v3/klines"
        params = {
            'symbol': symbol,
            'interval': interval,
            'limit': limit
        }
        
        try:
            async with self.session.get(url, params=params) as response:
                data = await response.json()
                
            df = pd.DataFrame(data, columns=[
                'timestamp', 'open', 'high', 'low', 'close', 'volume',
                'close_time', 'quote_asset_volume', 'number_of_trades',
                'taker_buy_base_asset_volume', 'taker_buy_quote_asset_volume', 'ignore'
            ])
            
            # Converte tipos
            numeric_columns = ['open', 'high', 'low', 'close', 'volume']
            df[numeric_columns] = df[numeric_columns].astype(float)
            df['timestamp'] = pd.to_datetime(df['timestamp'], unit='ms')
            
            # Define símbolo como atributo
            df.attrs['symbol'] = symbol
            
            return df[['timestamp', 'open', 'high', 'low', 'close', 'volume']]
            
        except Exception as e:
            self.logger.error(f"Erro ao coletar dados da Binance: {e}")
            return pd.DataFrame()
    
    async def get_alpha_vantage_data(self, symbol: str, api_key: str) -> pd.DataFrame:
        """Coleta dados da Alpha Vantage"""
        url = "https://www.alphavantage.co/query"
        params = {
            'function': 'TIME_SERIES_INTRADAY',
            'symbol': symbol,
            'interval': '5min',
            'apikey': api_key,
            'outputsize': 'compact'
        }
        
        try:
            async with self.session.get(url, params=params) as response:
                data = await response.json()
            
            time_series = data.get('Time Series (5min)', {})
            
            df_data = []
            for timestamp, values in time_series.items():
                df_data.append({
                    'timestamp': pd.to_datetime(timestamp),
                    'open': float(values['1. open']),
                    'high': float(values['2. high']),
                    'low': float(values['3. low']),
                    'close': float(values['4. close']),
                    'volume': float(values['5. volume'])
                })
            
            df = pd.DataFrame(df_data)
            df = df.sort_values('timestamp').reset_index(drop=True)
            df.attrs['symbol'] = symbol
            
            return df
            
        except Exception as e:
            self.logger.error(f"Erro ao coletar dados da Alpha Vantage: {e}")
            return pd.DataFrame()
    
    async def collect_multiple_assets(self, assets: List[Dict]) -> Dict[str, pd.DataFrame]:
        """Coleta dados de múltiplos ativos simultaneamente"""
        tasks = []
        
        for asset in assets:
            if asset['source'] == 'binance':
                task = self.get_binance_data(asset['symbol'])
            elif asset['source'] == 'alpha_vantage':
                task = self.get_alpha_vantage_data(asset['symbol'], asset['api_key'])
            
            tasks.append(task)
        
        results = await asyncio.gather(*tasks, return_exceptions=True)
        
        data_dict = {}
        for i, result in enumerate(results):
            if isinstance(result, pd.DataFrame) and not result.empty:
                data_dict[assets[i]['symbol']] = result
            else:
                self.logger.warning(f"Falha ao coletar dados para {assets[i]['symbol']}")
        
        return data_dict
```

## 4. Exemplo de Gerador de Sinais

```python
class SignalGenerator:
    """Gera e gerencia sinais de trading"""
    
    def __init__(self):
        self.strategies = []
        self.active_signals = []
        self.logger = logging.getLogger(__name__)
    
    def add_strategy(self, strategy: BaseStrategy):
        """Adiciona uma estratégia"""
        self.strategies.append(strategy)
        self.logger.info(f"Estratégia {strategy.name} adicionada")
    
    def remove_strategy(self, strategy_name: str):
        """Remove uma estratégia"""
        self.strategies = [s for s in self.strategies if s.name != strategy_name]
    
    async def generate_signals(self, market_data: Dict[str, pd.DataFrame]) -> List[Dict]:
        """Gera sinais para todos os ativos e estratégias"""
        new_signals = []
        
        for asset_symbol, data in market_data.items():
            if data.empty:
                continue
                
            for strategy in self.strategies:
                try:
                    signal = strategy.generate_signal(data)
                    if signal:
                        signal['asset'] = asset_symbol
                        new_signals.append(signal)
                        self.logger.info(f"Novo sinal: {signal['direction']} em {asset_symbol}")
                        
                except Exception as e:
                    self.logger.error(f"Erro ao gerar sinal para {asset_symbol} com {strategy.name}: {e}")
        
        # Filtra sinais duplicados
        filtered_signals = self.filter_duplicate_signals(new_signals)
        self.active_signals.extend(filtered_signals)
        
        return filtered_signals
    
    def filter_duplicate_signals(self, signals: List[Dict]) -> List[Dict]:
        """Remove sinais duplicados para o mesmo ativo"""
        seen_assets = set()
        filtered = []
        
        # Ordena por confiança (maior primeiro)
        signals.sort(key=lambda x: x['confidence'], reverse=True)
        
        for signal in signals:
            asset_key = f"{signal['asset']}_{signal['direction']}"
            if asset_key not in seen_assets:
                seen_assets.add(asset_key)
                filtered.append(signal)
        
        return filtered
    
    def get_signal_summary(self) -> Dict:
        """Retorna resumo dos sinais ativos"""
        if not self.active_signals:
            return {"total": 0, "calls": 0, "puts": 0, "avg_confidence": 0}
        
        calls = sum(1 for s in self.active_signals if s['direction'] == 'CALL')
        puts = sum(1 for s in self.active_signals if s['direction'] == 'PUT')
        avg_confidence = sum(s['confidence'] for s in self.active_signals) / len(self.active_signals)
        
        return {
            "total": len(self.active_signals),
            "calls": calls,
            "puts": puts,
            "avg_confidence": round(avg_confidence, 2)
        }
```

## 5. Exemplo de Notificador Telegram

```python
import asyncio
import aiohttp
from typing import Dict, List

class TelegramNotifier:
    """Envia notificações via Telegram"""
    
    def __init__(self, bot_token: str, chat_ids: List[str]):
        self.bot_token = bot_token
        self.chat_ids = chat_ids
        self.base_url = f"https://api.telegram.org/bot{bot_token}"
    
    async def send_signal(self, signal: Dict):
        """Envia sinal formatado para Telegram"""
        message = self.format_signal_message(signal)
        
        for chat_id in self.chat_ids:
            await self.send_message(chat_id, message)
    
    def format_signal_message(self, signal: Dict) -> str:
        """Formata mensagem do sinal"""
        direction_emoji = "📈" if signal['direction'] == 'CALL' else "📉"
        confidence_emoji = "🔥" if signal['confidence'] > 0.8 else "⚡" if signal['confidence'] > 0.6 else "💡"
        
        message = f"""
{confidence_emoji} <b>SINAL DE TRADING</b> {confidence_emoji}

{direction_emoji} <b>Ativo:</b> {signal['asset']}
📊 <b>Direção:</b> {signal['direction']}
⏰ <b>Timeframe:</b> {signal['timeframe']}
💰 <b>Entrada:</b> ${signal['entry_price']:.4f}
🛑 <b>Stop Loss:</b> ${signal['stop_loss']:.4f}
🎯 <b>Take Profit:</b> ${signal['take_profit']:.4f}
📈 <b>Confiança:</b> {signal['confidence']:.0%}
🤖 <b>Estratégia:</b> {signal['strategy']}

📊 <b>Indicadores:</b>
"""
        
        for indicator, value in signal['indicators'].items():
            if isinstance(value, (int, float)):
                message += f"• {indicator.upper()}: {value:.2f}\n"
            else:
                message += f"• {indicator.upper()}: {value}\n"
        
        message += f"\n⚠️ <i>Opere por sua conta e risco!</i>"
        
        return message
    
    async def send_message(self, chat_id: str, message: str):
        """Envia mensagem para chat específico"""
        url = f"{self.base_url}/sendMessage"
        data = {
            'chat_id': chat_id,
            'text': message,
            'parse_mode': 'HTML'
        }
        
        try:
            async with aiohttp.ClientSession() as session:
                async with session.post(url, json=data) as response:
                    if response.status != 200:
                        print(f"Erro ao enviar mensagem: {response.status}")
        except Exception as e:
            print(f"Erro no Telegram: {e}")
    
    async def send_daily_report(self, strategies_performance: Dict):
        """Envia relatório diário de performance"""
        message = "📊 <b>RELATÓRIO DIÁRIO</b>\n\n"
        
        for strategy_name, performance in strategies_performance.items():
            message += f"🤖 <b>{strategy_name}</b>\n"
            message += f"• Sinais gerados: {performance['signals_count']}\n"
            message += f"• Taxa de acerto: {performance['win_rate']:.1%}\n"
            message += f"• Profit/Loss: {performance['pnl']:+.2f}%\n\n"
        
        for chat_id in self.chat_ids:
            await self.send_message(chat_id, message)
```

## 6. Exemplo de Sistema Principal

```python
import asyncio
import logging
from datetime import datetime, timedelta

class TradingBot:
    """Sistema principal do bot de trading"""
    
    def __init__(self, config: Dict):
        self.config = config
        self.data_collector = DataCollector()
        self.signal_generator = SignalGenerator()
        self.notifier = TelegramNotifier(
            config['telegram_token'], 
            config['telegram_chats']
        )
        self.is_running = False
        self.logger = logging.getLogger(__name__)
        
        # Adiciona estratégias
        self.signal_generator.add_strategy(MeanReversionStrategy())
        # Adicione outras estratégias aqui
    
    async def start(self):
        """Inicia o bot"""
        self.is_running = True
        self.logger.info("Bot iniciado")
        
        while self.is_running:
            try:
                await self.trading_cycle()
                await asyncio.sleep(60)  # Executa a cada minuto
                
            except Exception as e:
                self.logger.error(f"Erro no ciclo de trading: {e}")
                await asyncio.sleep(30)
    
    async def trading_cycle(self):
        """Executa um ciclo completo de análise e geração de sinais"""
        # 1. Coleta dados
        async with self.data_collector as collector:
            market_data = await collector.collect_multiple_assets(
                self.config['assets']
            )
        
        if not market_data:
            self.logger.warning("Nenhum dado coletado")
            return
        
        # 2. Gera sinais
        new_signals = await self.signal_generator.generate_signals(market_data)
        
        # 3. Envia notificações
        for signal in new_signals:
            await self.notifier.send_signal(signal)
        
        # 4. Log do resumo
        summary = self.signal_generator.get_signal_summary()
        self.logger.info(f"Resumo: {summary}")
    
    async def stop(self):
        """Para o bot"""
        self.is_running = False
        self.logger.info("Bot parado")

# Configuração de exemplo
config = {
    'telegram_token': 'YOUR_BOT_TOKEN',
    'telegram_chats': ['CHAT_ID_1', 'CHAT_ID_2'],
    'assets': [
        {'symbol': 'BTCUSDT', 'source': 'binance'},
        {'symbol': 'ETHUSDT', 'source': 'binance'},
        {'symbol': 'AAPL', 'source': 'alpha_vantage', 'api_key': 'YOUR_API_KEY'}
    ]
}

# Execução do bot
async def main():
    bot = TradingBot(config)
    try:
        await bot.start()
    except KeyboardInterrupt:
        await bot.stop()

if __name__ == "__main__":
    logging.basicConfig(level=logging.INFO)
    asyncio.run(main())
```

Estes exemplos fornecem uma base sólida para implementação do bot de sinais. Cada componente pode ser expandido e customizado conforme suas necessidades específicas.
