using Inverse_CC_bot.DataAccess.Models;

namespace Inverse_CC_bot.Interfaces
{
    public interface ICoinSentimentsDAL
    {
        void InsertCoinSentiment(CoinSentiment coinSentiment);
        void RemoveCoinSentimentById(int id);
        List<CoinSentiment> GetAllCoinSentiments();
    }
}
