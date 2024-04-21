using ExchangeSharp;
using Inverse_CC_bot.Interfaces;
using Npgsql;
using System.Data;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class OrdersDAL : IOrdersDAL
    {
        private readonly DatabaseService _databaseService;

        public OrdersDAL(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void InsertOrder(ExchangeOrderResult order)
        {
            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            if (order == null)
            {
                return;
            }

            try
            {
                using NpgsqlCommand cmd = new(
                    "INSERT INTO orders (order_id, client_order_id, result, result_code, message, amount, amount_filled, " +
                    "is_amount_filled_reversed, price, average_price, order_date, http_header_date, completed_date, market_symbol, is_buy, fees, fees_currency, trade_id, trade_date, update_sequence) " +
                    "VALUES (@OrderId, @ClientOrderId, @Result, @ResultCode, @Message, @Amount, @AmountFilled, @IsAmountFilledReversed, @Price, " +
                    "@AveragePrice, @OrderDate, @HTTPHeaderDate, @CompletedDate, @MarketSymbol, @IsBuy, @Fees, @FeesCurrency, @TradeId, " +
                    "@TradeDate, @UpdateSequence)",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("OrderId", order.OrderId);
                cmd.Parameters.AddWithValue("ClientOrderId", order.ClientOrderId ?? null);
                cmd.Parameters.AddWithValue("Result", (int)order.Result);
                cmd.Parameters.AddWithValue("ResultCode", order.ResultCode);
                cmd.Parameters.AddWithValue("Message", order.Message);
                cmd.Parameters.AddWithValue("Amount", order.Amount);
                cmd.Parameters.AddWithValue("AmountFilled", order.AmountFilled ?? 0);
                cmd.Parameters.AddWithValue("IsAmountFilledReversed", order.IsAmountFilledReversed);
                cmd.Parameters.AddWithValue("Price", order.Price ?? 0);
                cmd.Parameters.AddWithValue("AveragePrice", order.AveragePrice ?? 0);
                cmd.Parameters.AddWithValue("OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("HTTPHeaderDate", order.HTTPHeaderDate ?? DateTime.Now);
                cmd.Parameters.AddWithValue("CompletedDate", order.CompletedDate ?? DateTime.Now);
                cmd.Parameters.AddWithValue("MarketSymbol", order.MarketSymbol);
                cmd.Parameters.AddWithValue("IsBuy", order.IsBuy);
                cmd.Parameters.AddWithValue("Fees", order.Fees ?? 0);
                cmd.Parameters.AddWithValue("FeesCurrency", order.FeesCurrency);
                cmd.Parameters.AddWithValue("TradeId", order.TradeId);
                cmd.Parameters.AddWithValue("TradeDate", order.TradeDate ?? DateTime.Now);
                cmd.Parameters.AddWithValue("UpdateSequence", order.UpdateSequence ?? 0);

                cmd.ExecuteNonQuery();

                _databaseService.CommitTransaction();
            }
            catch (Exception ex)
            {
                _databaseService.RollbackTransaction();
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }
        }

        public ExchangeOrderResult? GetOrderByOrderId(string orderId)
        {
            ExchangeOrderResult order = null;

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new(
                    "SELECT * FROM orders WHERE order_id = @OrderId",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("OrderId", orderId);

                using NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    order = new ExchangeOrderResult
                    {
                        OrderId = reader.GetString("order_id"),
                        ClientOrderId = reader.GetString("client_order_id"),
                        Result = (ExchangeAPIOrderResult)reader.GetInt32("result"),
                        ResultCode = reader.GetString("result_code"),
                        Message = reader.GetString("message"),
                        Amount = reader.GetDecimal("amount"),
                        AmountFilled = reader.IsDBNull("amount_filled") ? null : (decimal?)reader.GetDecimal("amount_filled"),
                        IsAmountFilledReversed = reader.GetBoolean("is_amount_filled_reversed"),
                        Price = reader.IsDBNull("price") ? null : (decimal?)reader.GetDecimal("price"),
                        AveragePrice = reader.IsDBNull("average_price") ? null : (decimal?)reader.GetDecimal("average_price"),
                        OrderDate = reader.GetDateTime("order_date"),
                        HTTPHeaderDate = reader.IsDBNull("http_header_date") ? null : (DateTime?)reader.GetDateTime("http_header_date"),
                        CompletedDate = reader.IsDBNull("completed_date") ? null : (DateTime?)reader.GetDateTime("completed_date"),
                        MarketSymbol = reader.GetString("market_symbol"),
                        IsBuy = reader.GetBoolean("is_buy"),
                        Fees = reader.IsDBNull("fees") ? null : (decimal?)reader.GetDecimal("fees"),
                        FeesCurrency = reader.GetString("fees_currency"),
                        TradeId = reader.GetString("trade_id"),
                        TradeDate = reader.IsDBNull("trade_date") ? null : (DateTime?)reader.GetDateTime("trade_date"),
                        UpdateSequence = reader.IsDBNull("update_sequence") ? null : (long?)reader.GetInt64("update_sequence")
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }

            return order;
        }

        public List<ExchangeOrderResult> GetAllOrders()
        {
            List<ExchangeOrderResult> orders = new();

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new("SELECT * FROM orders", _databaseService.connection);

                using NpgsqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ExchangeOrderResult order = new()
                    {
                        OrderId = reader.GetString("order_id"),
                        ClientOrderId = reader.GetString("client_order_id"),
                        Result = (ExchangeAPIOrderResult)reader.GetInt32("result"),
                        ResultCode = reader.GetString("result_code"),
                        Message = reader.GetString("message"),
                        Amount = reader.GetDecimal("amount"),
                        AmountFilled = reader.IsDBNull("amount_filled") ? null : (decimal?)reader.GetDecimal("amount_filled"),
                        IsAmountFilledReversed = reader.GetBoolean("is_amount_filled_reversed"),
                        Price = reader.IsDBNull("price") ? null : (decimal?)reader.GetDecimal("price"),
                        AveragePrice = reader.IsDBNull("average_price") ? null : (decimal?)reader.GetDecimal("average_price"),
                        OrderDate = reader.GetDateTime("order_date"),
                        HTTPHeaderDate = reader.IsDBNull("http_header_date") ? null : (DateTime?)reader.GetDateTime("http_header_date"),
                        CompletedDate = reader.IsDBNull("completed_date") ? null : (DateTime?)reader.GetDateTime("completed_date"),
                        MarketSymbol = reader.GetString("market_symbol"),
                        IsBuy = reader.GetBoolean("is_buy"),
                        Fees = reader.IsDBNull("fees") ? null : (decimal?)reader.GetDecimal("fees"),
                        FeesCurrency = reader.GetString("fees_currency"),
                        TradeId = reader.GetString("trade_id"),
                        TradeDate = reader.IsDBNull("trade_date") ? null : (DateTime?)reader.GetDateTime("trade_date"),
                        UpdateSequence = reader.IsDBNull("update_sequence") ? null : (long?)reader.GetInt64("update_sequence")
                    };

                    orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }

            return orders;
        }
    }
}
