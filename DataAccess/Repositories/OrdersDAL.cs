using Dapper;
using ExchangeSharp;
using Inverse_CC_bot.Interfaces;
using Npgsql;
using System.Data;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class OrdersDAL : IOrdersDAL
    {
        private readonly string _connectionString;

        public OrdersDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public void InsertOrder(ExchangeOrderResult order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            const string query = @"
                INSERT INTO orders 
                (
                    order_id, client_order_id, result, result_code, message, amount, amount_filled, 
                    is_amount_filled_reversed, price, average_price, order_date, http_header_date, 
                    completed_date, market_symbol, is_buy, fees, fees_currency, trade_id, 
                    trade_date, update_sequence
                )
                VALUES 
                (
                    @OrderId, @ClientOrderId, @Result, @ResultCode, @Message, @Amount, @AmountFilled, 
                    @IsAmountFilledReversed, @Price, @AveragePrice, @OrderDate, @HTTPHeaderDate, 
                    @CompletedDate, @MarketSymbol, @IsBuy, @Fees, @FeesCurrency, @TradeId, 
                    @TradeDate, @UpdateSequence
                )";

            using var connection = GetConnection();
            connection.Execute(query, order);
        }

        public ExchangeOrderResult? GetOrderByOrderId(string orderId)
        {
            const string query = "SELECT * FROM orders WHERE order_id = @OrderId";

            using var connection = GetConnection();
            return connection.QueryFirstOrDefault<ExchangeOrderResult>(query, new { OrderId = orderId });
        }

        public List<ExchangeOrderResult> GetOrdersBySymbol(string symbol)
        {
            const string query = "SELECT * FROM orders WHERE market_symbol = @Symbol";

            using var connection = GetConnection();
            return connection.Query<ExchangeOrderResult>(query, new { Symbol = symbol }).ToList();
        }

        public List<ExchangeOrderResult> GetAllOrders()
        {
            const string query = "SELECT * FROM orders";

            using var connection = GetConnection();
            return connection.Query<ExchangeOrderResult>(query).ToList();
        }
    }
}
