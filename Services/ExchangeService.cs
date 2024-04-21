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

        public async Task<ExchangeOrderResult> PlacePaperOrder(string symbol, int quantity)
        {
            var ticker = await _exchangeAPI.GetTickerAsync(symbol);
            var convertedQuantity = await ConvertQuantity(quantity, symbol);


            return new ExchangeOrderResult
            {
                OrderId = Guid.NewGuid().ToString(),
                ClientOrderId = Guid.NewGuid().ToString(),
                Result = ExchangeAPIOrderResult.Filled,
                ResultCode = "200",
                Message = "Paper Order",
                Amount = convertedQuantity ?? 0,
                AmountFilled = convertedQuantity ?? 0,
                IsAmountFilledReversed = false,
                Price = ticker.Ask,
                AveragePrice = ticker.Ask,
                OrderDate = DateTime.UtcNow,
                HTTPHeaderDate = DateTime.UtcNow,
                MarketSymbol = symbol,
                IsBuy = true,
                Fees = convertedQuantity * 0.015m,
                FeesCurrency = symbol,
                TradeId = Guid.NewGuid().ToString(),
                TradeDate = DateTime.UtcNow,
                UpdateSequence = 0,

            };
        }

        public async Task<int> CalculateOrderPrecision(string stepSize)
        {
            string? stepSizeDigits = null;

            //avoid erroring out on integers
            try
            {
                stepSizeDigits = stepSize.Split('.')[1];
            }
            catch
            {
                Console.WriteLine("Step size is integer, returning 0");
            }

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


            if (symbolInfo == null)
            {
                return null;
            }

            return symbolInfo.QuantityStepSize.ToString() ?? null;
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
