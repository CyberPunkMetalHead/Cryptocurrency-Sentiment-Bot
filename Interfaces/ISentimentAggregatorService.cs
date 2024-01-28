using Inverse_CC_bot.DataAccess.Models;

namespace Inverse_CC_bot.Interfaces
{
    public interface ISentimentAggregatorService
    {
        Dictionary<string, List<double>>? AggregateSentiments(List<RedditPost> redditPosts);
        double CalculateAverageSentiment(List<double> sentimentValues);
    }
}
