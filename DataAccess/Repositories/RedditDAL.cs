using Dapper;
using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using System.Data;
using Npgsql;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class RedditDAL : IRedditDAL
    {
        private readonly string _connectionString;

        public RedditDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public void InsertRedditPost(RedditPost post)
        {
            const string query = @"
                INSERT INTO reddit_posts (post_title, post_id, date_time_posted, upvotes, num_comments, description, url) 
                VALUES (@PostTitle, @PostId, @DateTimePosted, @Upvotes, @NumComments, @Description, @URL)
                ON CONFLICT (post_id) DO NOTHING";

            using var connection = GetConnection();
            connection.Execute(query, post);
        }

        public void InsertRedditPosts(List<RedditPost> posts)
        {
            if (posts == null || posts.Count == 0) return;

            const string query = @"
                INSERT INTO reddit_posts (post_title, post_id, date_time_posted, upvotes, num_comments, description, url) 
                VALUES (@PostTitle, @PostId, @DateTimePosted, @Upvotes, @NumComments, @Description, @URL)
                ON CONFLICT (post_id) DO NOTHING";

            using var connection = GetConnection();
            connection.Execute(query, posts);
        }

        public List<RedditPost> GetAllRedditPosts()
        {
            const string query = "SELECT * FROM reddit_posts";

            using var connection = GetConnection();
            return connection.Query<RedditPost>(query).ToList();
        }

        public List<RedditPost> GetRedditPostsWithoutSentiment()
        {
            const string query = "SELECT * FROM reddit_posts WHERE sentiment_score IS NULL";

            using var connection = GetConnection();
            return connection.Query<RedditPost>(query).ToList();
        }

        public List<RedditPost> GetRedditPostsWithSentiment()
        {
            const string query = "SELECT * FROM reddit_posts WHERE sentiment_score IS NOT NULL";

            using var connection = GetConnection();
            return connection.Query<RedditPost>(query).ToList();
        }

        public void UpdatePostSentiment(RedditPost post)
        {
            if (post == null || post.SentimentScore == null) return;

            const string query = "UPDATE reddit_posts SET sentiment_score = @SentimentScore WHERE id = @Id";

            using var connection = GetConnection();
            connection.Execute(query, new { SentimentScore = post.SentimentScore, Id = post.Id });
        }

        public void UpdateTopicDiscussed(RedditPost post)
        {
            if (post == null || post.TopicDiscussed == null) return;

            const string query = "UPDATE reddit_posts SET topic_discussed = @TopicDiscussed WHERE id = @Id";

            using var connection = GetConnection();
            connection.Execute(query, new { TopicDiscussed = post.TopicDiscussed, Id = post.Id });
        }
    }
}
