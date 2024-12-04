using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Dapper;
using Npgsql;
using System.Data;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class StatisticsDAL : IStatisticsDAL
    {
        private readonly string _connectionString;

        public StatisticsDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public void InsertStatisticsItem(StatisticsItem statistic)
        {
            const string query = "INSERT INTO statistics (symbol, pnl, date) VALUES (@Symbol, @Pnl, @Date)";

            using var connection = GetConnection();
            connection.Execute(query, statistic); 
        }

        public void UpdateStatisticsItemBySymbol(string symbol, decimal pnl, DateTime date)
        {
            const string query = "UPDATE statistics SET pnl = @Pnl WHERE symbol = @Symbol AND date = @Date";

            using var connection = GetConnection();
            var rowsAffected = connection.Execute(query, new { Symbol = symbol, Pnl = pnl, Date = date });

            if (rowsAffected == 0)
            {
                throw new Exception("No rows were updated. Please check if the symbol exists.");
            }
        }

        public StatisticsItem GetStatisticsBySymbol(string symbol)
        {
            const string query = "SELECT symbol, pnl FROM statistics WHERE symbol = @Symbol LIMIT 1";

            using var connection = GetConnection();
            return connection.QueryFirstOrDefault<StatisticsItem>(query, new { Symbol = symbol });
        }
    }
}