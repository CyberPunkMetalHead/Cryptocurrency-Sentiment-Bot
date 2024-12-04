using Dapper;
using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Npgsql;
using System.Data;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class CoinsDAL : ICoinsDAL
    {
        private readonly string _connectionString;

        public CoinsDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public List<Coin> GetAllCoins()
        {
            const string query = "SELECT * FROM coins";

            using var connection = GetConnection();
            return connection.Query<Coin>(query).ToList();
        }
    }
}