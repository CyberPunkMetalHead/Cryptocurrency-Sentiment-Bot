using Dapper;
using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Npgsql;
using System.Data;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class CoinSentimentsDAL : ICoinSentimentsDAL
    {
        private readonly string _connectionString;

        public CoinSentimentsDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public void InsertCoinSentiment(CoinSentiment coinSentiment)
        {
            const string query = @"
                INSERT INTO coin_sentiments (symbol, date, sentiment_value) 
                VALUES (@Symbol, @Date, @SentimentValue)";

            using var connection = GetConnection();
            connection.Execute(query, coinSentiment);
        }

        public void RemoveCoinSentimentById(int id)
        {
            const string query = "DELETE FROM coin_sentiments WHERE id = @Id";

            using var connection = GetConnection();
            connection.Execute(query, new { Id = id });
        }

        public List<CoinSentiment> GetAllCoinSentiments()
        {
            const string query = "SELECT * FROM coin_sentiments";

            using var connection = GetConnection();
            return connection.Query<CoinSentiment>(query).ToList();
        }
    }
}