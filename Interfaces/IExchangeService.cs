using ExchangeSharp;

namespace Inverse_CC_bot.Services
{
    public interface IExchangeService
    {
        Task<ExchangeOrderResult?> PlaceOrder(string symbol, int quantity);

        Task<int> CalculateOrderPrecision(string stepSize);

        Task<string?> GetLotSize(string symbol);

        Task<decimal?> ConvertQuantity(int quantity, string symbol);
    }
}
