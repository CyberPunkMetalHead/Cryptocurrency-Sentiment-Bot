using ExchangeSharp;
using Inverse_CC_bot.Enums;

namespace Inverse_CC_bot.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly IExchangeAPI _exchangeAPI;

        public ExchangeService(ExchangeNameEnum exchangeName, string apiKey, string apiSecret)
        {

            // Initialize the Exchange API within the constructor
            _exchangeAPI = ExchangeAPI.GetExchangeAPIAsync(exchangeName.ToString()).Result;
            _exchangeAPI.LoadAPIKeysUnsecure(apiKey, apiSecret);

        }

        public async Task<ExchangeOrderResult?> PlaceOrder(string symbol, int quantity)
        {

            var ticker = await _exchangeAPI.GetTickerAsync(symbol);
            var convertedQuantity = await ConvertQuantity(quantity, symbol);

            if (convertedQuantity == null)
            {
                return null;
            }

            var result = await _exchangeAPI.PlaceOrderAsync(
                    new ExchangeOrderRequest
                    {
                        Amount = convertedQuantity.Value,
                        IsBuy = true,
                        Price = ticker.Ask,
                        MarketSymbol = symbol
                    }
            );

            await Task.Delay(500);

            return result;
        }


        public async Task<int> CalculateOrderPrecision(string stepSize)
        {
            var stepSizeDigits = stepSize.Split('.')[1];

            if (!string.IsNullOrWhiteSpace(stepSizeDigits))
            {
                var nonZeroIndex = stepSizeDigits.IndexOf('1');
                return nonZeroIndex + 1;
            }

            return 0; // If stepSizeDigits is not available (stepSize is an integer), precision is 0
        }

        public async Task<string?> GetLotSize(string symbol)
        {
            var exchangeInfo = await _exchangeAPI.GetMarketSymbolsMetadataAsync();
            var symbolInfo = exchangeInfo.FirstOrDefault(x => x.MarketSymbol == symbol);

            if (symbolInfo.QuantityStepSize == null)
            {
                return null;
            }

            return symbolInfo.QuantityStepSize.ToString();
        }

        public async Task<decimal?> ConvertQuantity(int quantity, string symbol)
        {
            var lotSize = await GetLotSize(symbol);
            if (lotSize == null)
            {
                return null;
            }

            var ticker = await _exchangeAPI.GetTickerAsync(symbol);
            var precision = await CalculateOrderPrecision(lotSize);

            return precision == 0
                ? Convert.ToInt64(quantity / ticker.Ask)
                : Math.Round((quantity / ticker.Ask), precision);
        }

    }
}
