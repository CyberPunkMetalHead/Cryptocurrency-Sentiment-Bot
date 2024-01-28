using Inverse_CC_bot.DataAccess.Models;

namespace Inverse_CC_bot.Interfaces
{
    public interface IRedditDAL
    {
        void InsertRedditPost(RedditPost post);

        void InsertRedditPosts(List<RedditPost> post);

        void UpdatePostSentiment(RedditPost post);

        void UpdateTopicDiscussed(RedditPost post);

        List<RedditPost> GetAllRedditPosts();

        List<RedditPost> GetRedditPostsWithoutSentiment();

        List<RedditPost> GetRedditPostsWithSentiment();



    }
}
