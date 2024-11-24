using ExchangeSharp;
using Inverse_CC_bot.DataAccess.Models;

namespace Inverse_CC_bot.Interfaces
{
    public interface IStatisticsService
    {
        decimal CalculatePNL(ExchangeOrderResult order, decimal currentPrice);
        decimal AggregatePNL(List<PortfolioItem> portfolioItems, decimal currentPrice);
    }
}