using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Npgsql;
using System.Data;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class CoinSentimentsDAL : ICoinSentimentsDAL
    {
        public DatabaseService _databaseService;

        public CoinSentimentsDAL(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void InsertCoinSentiment(CoinSentiment coinSentiment)
        {
            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            try
            {
                using NpgsqlCommand cmd = new NpgsqlCommand(
                    "INSERT INTO coin_sentiments (symbol, date, sentiment_value) " +
                    "VALUES (@Symbol, @Date, @SentimentValue)",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("Symbol", coinSentiment.Symbol);
                cmd.Parameters.AddWithValue("Date", coinSentiment.Date);
                cmd.Parameters.AddWithValue("SentimentValue", coinSentiment.SentimentValue);

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

        public void RemoveCoinSentimentById(int id)
        {
            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            try
            {
                using NpgsqlCommand cmd = new NpgsqlCommand(
                    "DELETE FROM coin_sentiments WHERE id = @Id",
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

        public List<CoinSentiment> GetAllCoinSentiments()
        {
            List<CoinSentiment> coinSentiments = new();

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM coin_sentiments", _databaseService.connection);

                using NpgsqlDataReader reader = cmd.ExecuteReader();

                coinSentiments = reader.Cast<IDataRecord>()
                    .Select(r => new CoinSentiment
                    {
                        Id = r.GetInt32(r.GetOrdinal("id")),
                        Symbol = r.GetString(r.GetOrdinal("symbol")),
                        Date = r.GetDateTime(r.GetOrdinal("date")),
                        SentimentValue = r.GetDouble(r.GetOrdinal("sentiment_value"))
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

            return coinSentiments;
        }
    }
}
