using Inverse_CC_bot.DataAccess.Models;

namespace Inverse_CC_bot.Interfaces
{
    public interface IRedditScrapingService
    {
        Task<List<RedditPost>> GetRedditPosts(string subreddit, string filter, int count);
    }
}
