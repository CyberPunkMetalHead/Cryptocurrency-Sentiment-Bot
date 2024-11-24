using Inverse_CC_bot.DataAccess.Models;

namespace Inverse_CC_bot.Interfaces
{
    public interface IStatisticsDAL
    {
        StatisticsItem GetStatisticsBySymbol(string symbol);
        void InsertStatisticsItem(StatisticsItem statistic);
        void UpdateStatisticsItemBySymbol(string symbol, decimal pnl, DateTime date);
    }
}