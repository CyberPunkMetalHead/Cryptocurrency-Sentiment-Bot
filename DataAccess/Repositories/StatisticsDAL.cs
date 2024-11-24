using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Npgsql;
using System;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class StatisticsDAL : IStatisticsDAL
    {
        private readonly DatabaseService _databaseService;

        public StatisticsDAL(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void InsertStatisticsItem(StatisticsItem statistic)
        {
            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            try
            {
                using NpgsqlCommand cmd = new(
                    "INSERT INTO statistics (symbol, pnl, date) " +
                    "VALUES (@Symbol, @Pnl, @Date)",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("Symbol", statistic.Symbol);
                cmd.Parameters.AddWithValue("Pnl", statistic.Pnl);
                cmd.Parameters.AddWithValue("Date", statistic.Date);

                cmd.ExecuteNonQuery();

                _databaseService.CommitTransaction();
            }
            catch (Exception)
            {
                _databaseService.RollbackTransaction();
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }
        }

        public void UpdateStatisticsItemBySymbol(string symbol, decimal pnl, DateTime date)
        {
            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            try
            {
                using NpgsqlCommand cmd = new(
                    "UPDATE statistics " +
                    "SET pnl = @Pnl " +
                    "WHERE symbol = @Symbol and date = @Date",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("Pnl", pnl);
                cmd.Parameters.AddWithValue("Symbol", symbol);
                cmd.Parameters.AddWithValue("Date", date);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    // Handle case where no rows were updated (symbol not found)
                    throw new Exception("No rows were updated. Please check if the symbol exists.");
                }

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
        
        public StatisticsItem GetStatisticsBySymbol(string symbol)
        {
            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new(
                    "SELECT symbol, pnl FROM statistics WHERE symbol = @Symbol LIMIT 1",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("Symbol", symbol);

                using NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new StatisticsItem
                    {
                        Symbol = reader.GetString(0),
                        Pnl = reader.GetDecimal(1)
                    };
                }

                return null; 
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }
        }
    }
}