using Inverse_CC_bot.DataAccess.Models;
using System.Collections.Generic;

namespace Inverse_CC_bot.Interfaces
{
    public interface ICoinsDAL
    {
        List<Coin> GetAllCoins();
    }
}
