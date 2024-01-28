using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;

namespace Inverse_CC_bot.Services
{
    public class SentimentAggregatorService : ISentimentAggregatorService
    {
        public Dictionary<string, List<double>>? AggregateSentiments(List<RedditPost> redditPosts)
        {
            Dictionary<string, List<double>> CoinsSentimentDict = new();

            foreach (var post in redditPosts)
            {
                if (post.TopicDiscussed == null || post.SentimentScore == null) continue;

                string[] topics = post.TopicDiscussed.Split(',');

                foreach (var coin in topics)
                {
                    if (!CoinsSentimentDict.ContainsKey(coin.Trim()))
                    {
                        CoinsSentimentDict[coin.Trim()] = new List<double>();
                    }

                    CoinsSentimentDict[coin.Trim()].Add(post.SentimentScore ?? 0.0);
                }
            }

            return CoinsSentimentDict;
        }

        public double CalculateAverageSentiment(List<double> sentimentValues)
        {
            if (sentimentValues.Count == 0)
            {
                return 0;
            }

            double sum = 0;
            foreach (var value in sentimentValues)
            {
                sum += value;
            }

            return sum / sentimentValues.Count;
        }
    }
}
