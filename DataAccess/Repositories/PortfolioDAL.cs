using Dapper;
using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Npgsql;
using System.Data;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class PortfolioDAL : IPortfolioDAL
    {
        private readonly string _connectionString;

        public PortfolioDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public void InsertPortfolioItem(PortfolioItem portfolioItem)
        {
            const string query = @"
                INSERT INTO portfolio (order_id, pnl, symbol) 
                VALUES (@OrderId, @Pnl, @Symbol)";

            using var connection = GetConnection();
            connection.Execute(query, portfolioItem);
        }

        public void RemovePortfolioItemById(int id)
        {
            const string query = "DELETE FROM portfolio WHERE id = @Id";

            using var connection = GetConnection();
            connection.Execute(query, new { Id = id });
        }

        public List<PortfolioItem> GetAllPortfolioItems()
        {
            const string query = "SELECT * FROM portfolio";

            using var connection = GetConnection();
            return connection.Query<PortfolioItem>(query).ToList();
        }

        public void UpdatePortfolioPnlById(int id, decimal pnl)
        {
            const string query = @"
                UPDATE portfolio 
                SET pnl = @Pnl 
                WHERE id = @Id";

            using var connection = GetConnection();
            connection.Execute(query, new { Id = id, Pnl = pnl });
        }
    }
}