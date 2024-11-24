using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Npgsql;
using System.Data;

namespace Inverse_CC_bot.DataAccess.Repositories
{

    public class PortfolioDAL : IPortfolioDAL
    {
        private readonly DatabaseService _databaseService;

        public PortfolioDAL(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void InsertPortfolioItem(PortfolioItem portfolioItem)
        {
            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            try
            {
                using NpgsqlCommand cmd = new(
                    "INSERT INTO portfolio (order_id, pnl, symbol) " +
                    "VALUES (@OrderId, @Pnl , @Symbol)",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("OrderId", portfolioItem.OrderId);
                cmd.Parameters.AddWithValue("Pnl", portfolioItem.Pnl ?? 0);
                cmd.Parameters.AddWithValue("Symbol", portfolioItem.Symbol);
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

        public void RemovePortfolioItemById(int id)
        {
            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            try
            {
                using NpgsqlCommand cmd = new(
                    "DELETE FROM portfolio WHERE id = @Id",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("Id", id);

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

        public List<PortfolioItem> GetAllPortfolioItems()
        {
            List<PortfolioItem> portfolioItems = new();

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new("SELECT * FROM portfolio", _databaseService.connection);
                using NpgsqlDataReader reader = cmd.ExecuteReader();

                portfolioItems = reader.Cast<IDataRecord>()
                    .Select(r => new PortfolioItem
                    {
                        Id = r.GetInt32(r.GetOrdinal("id")),
                        OrderId = r.GetString(r.GetOrdinal("order_id")),
                        Symbol = r.GetString(r.GetOrdinal("symbol")),
                        Pnl = r.GetDecimal(r.GetOrdinal("pnl"))
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }

            return portfolioItems;
        }
        
        public void UpdatePortfolioPnlById(int id, decimal pnl)
        {
            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            try
            {
                using NpgsqlCommand cmd = new(
                    "UPDATE portfolio " +
                    "SET pnl = @Pnl " +
                    "WHERE id = @Id",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("Id", id);
                cmd.Parameters.AddWithValue("Pnl", pnl);

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

    }
}
