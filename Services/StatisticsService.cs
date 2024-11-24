using ExchangeSharp;
using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;

namespace Inverse_CC_bot.Services
{
    public class StatisticsService: IStatisticsService
    {
        public decimal CalculatePNL(ExchangeOrderResult order, decimal currentPrice)
        {
            return ((currentPrice - order.Price) / order.Price) * 100 ?? 0;
        }

        public decimal AggregatePNL(List<PortfolioItem> portfolioItems, decimal currentPrice)
        {
            if (portfolioItems == null || portfolioItems.Count == 0)
            {
                return 0;
            }

            decimal totalPNL = 0;

            foreach (var portfolioItem in portfolioItems)
            {
                totalPNL += portfolioItem.Pnl ?? 0;
            }

            return totalPNL / portfolioItems.Count;
        }
    }
}