using Inverse_CC_bot.DataAccess.Models;
using Npgsql;
using System.Data;
using Inverse_CC_bot.Interfaces;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class CoinsDAL : ICoinsDAL
    {
        public DatabaseService _databaseService;

        public CoinsDAL(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public List<Coin> GetAllCoins()
        {
            List<Coin> coins = new();

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM coins", _databaseService.connection);

                using NpgsqlDataReader reader = cmd.ExecuteReader();

                coins = reader.Cast<IDataRecord>()
                    .Select(r => new Coin
                    {
                        Name = r.GetString(r.GetOrdinal("name")),
                        Symbol = r.GetString(r.GetOrdinal("symbol"))
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

            return coins;
        }
    }
}
