using Inverse_CC_bot.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inverse_CC_bot.Interfaces
{
    public interface IPortfolioDAL
    {
        void InsertPortfolioItem(PortfolioItem portfolioItem);
        void RemovePortfolioItemById(int id);
        List<PortfolioItem> GetAllPortfolioItems();
        void UpdatePortfolioPnlById(int id, decimal pnl);
    }
}
