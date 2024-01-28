using ExchangeSharp;

namespace Inverse_CC_bot.Interfaces
{
    public interface IOrdersDAL
    {
        void InsertOrder(ExchangeOrderResult order);
        ExchangeOrderResult? GetOrderByOrderId(string orderId);
        List<ExchangeOrderResult> GetAllOrders();
    }
}
