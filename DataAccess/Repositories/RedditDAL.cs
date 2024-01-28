using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Npgsql;
using System.Data;

namespace Inverse_CC_bot.DataAccess.Repositories
{
    public class RedditDAL : IRedditDAL
    {
        public DatabaseService _databaseService;

        public RedditDAL(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public void InsertRedditPost(RedditPost post)
        {
            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            try
            {
                using NpgsqlCommand cmd = new(
                    "INSERT INTO reddit_posts (post_title, post_id date_time_posted, upvotes, num_comments, description, url) " +
                    "VALUES (@PostTitle, @PostId @DateTimePosted, @Upvotes, @NumComments, @Description, @URL) ON CONFLICT (post_id) DO NOTHING",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("PostTitle", post.PostTitle);
                cmd.Parameters.AddWithValue("PostId", post.PostId);
                cmd.Parameters.AddWithValue("DateTimePosted", post.DateTimePosted);
                cmd.Parameters.AddWithValue("Upvotes", post.Upvotes);
                cmd.Parameters.AddWithValue("NumComments", post.NumComments);
                cmd.Parameters.AddWithValue("Description", post.Description);
                cmd.Parameters.AddWithValue("URL", post.URL);

                cmd.ExecuteNonQuery();

                _databaseService.CommitTransaction();
            }
            catch (Exception ex)
            {
                _databaseService.RollbackTransaction();
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }
        }
        public void InsertRedditPosts(List<RedditPost> posts)
        {
            if (posts == null || posts.Count == 0)
                return;

            _databaseService.OpenConnection();
            _databaseService.BeginTransaction();

            try
            {
                foreach (var post in posts)
                {
                    using var cmd = new NpgsqlCommand(
                        "INSERT INTO reddit_posts (post_title, post_id, date_time_posted, upvotes, num_comments, description, url) " +
                        "VALUES (@PostTitle, @PostId, @DateTimePosted, @Upvotes, @NumComments, @Description, @URL) ON CONFLICT (post_id) DO NOTHING",
                        _databaseService.connection);

                    cmd.Parameters.AddWithValue("PostTitle", post.PostTitle);
                    cmd.Parameters.AddWithValue("PostId", post.PostId);
                    cmd.Parameters.AddWithValue("DateTimePosted", post.DateTimePosted);
                    cmd.Parameters.AddWithValue("Upvotes", post.Upvotes);
                    cmd.Parameters.AddWithValue("NumComments", post.NumComments);
                    cmd.Parameters.AddWithValue("Description", post.Description);
                    cmd.Parameters.AddWithValue("URL", post.URL);

                    cmd.ExecuteNonQuery();
                }

                _databaseService.CommitTransaction();
            }
            catch (Exception ex)
            {
                _databaseService.RollbackTransaction();
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }
        }

        public List<RedditPost> GetAllRedditPosts()
        {
            List<RedditPost> redditPosts = new();

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM reddit_posts", _databaseService.connection);

                using NpgsqlDataReader reader = cmd.ExecuteReader();

                redditPosts = reader.Cast<IDataRecord>()
                    .Select(r => new RedditPost
                    {
                        Id = r.GetInt32(r.GetOrdinal("id")),
                        PostTitle = r.GetString(r.GetOrdinal("post_title")),
                        PostId = r.GetString(r.GetOrdinal("post_id")),
                        DateTimePosted = r.GetDateTime(r.GetOrdinal("date_time_posted")),
                        Upvotes = r.GetInt32(r.GetOrdinal("upvotes")),
                        NumComments = r.GetInt32(r.GetOrdinal("num_comments")),
                        Description = r.GetString(r.GetOrdinal("description")),
                        URL = r.GetString(r.GetOrdinal("url")),
                        TopicDiscussed = r.IsDBNull(r.GetOrdinal("topic_discussed")) ? null : r.GetString(r.GetOrdinal("topic_discussed")),
                        SentimentScore = r.IsDBNull(r.GetOrdinal("sentiment_score")) ? (double?)null : r.GetDouble(r.GetOrdinal("sentiment_score"))
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }

            return redditPosts;
        }

        public List<RedditPost> GetRedditPostsWithoutSentiment()
        {
            List<RedditPost> redditPosts = new();

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new("SELECT * FROM reddit_posts WHERE sentiment_score IS NULL", _databaseService.connection);

                using NpgsqlDataReader reader = cmd.ExecuteReader();

                redditPosts = reader.Cast<IDataRecord>()
                    .Select(r => new RedditPost
                    {
                        Id = r.GetInt32(r.GetOrdinal("id")),
                        PostTitle = r.GetString(r.GetOrdinal("post_title")),
                        PostId = r.GetString(r.GetOrdinal("post_id")),
                        DateTimePosted = r.GetDateTime(r.GetOrdinal("date_time_posted")),
                        Upvotes = r.GetInt32(r.GetOrdinal("upvotes")),
                        NumComments = r.GetInt32(r.GetOrdinal("num_comments")),
                        Description = r.GetString(r.GetOrdinal("description")),
                        URL = r.GetString(r.GetOrdinal("url")),
                        TopicDiscussed = r.IsDBNull(r.GetOrdinal("topic_discussed")) ? null : r.GetString(r.GetOrdinal("topic_discussed")),
                        SentimentScore = r.IsDBNull(r.GetOrdinal("sentiment_score")) ? (double?)null : r.GetDouble(r.GetOrdinal("sentiment_score"))
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }

            return redditPosts;
        }

        public List<RedditPost> GetRedditPostsWithSentiment()
        {
            List<RedditPost> redditPosts = new();

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new("SELECT * FROM reddit_posts WHERE sentiment_score IS NOT NULL", _databaseService.connection);

                using NpgsqlDataReader reader = cmd.ExecuteReader();

                redditPosts = reader.Cast<IDataRecord>()
                    .Select(r => new RedditPost
                    {
                        Id = r.GetInt32(r.GetOrdinal("id")),
                        PostTitle = r.GetString(r.GetOrdinal("post_title")),
                        PostId = r.GetString(r.GetOrdinal("post_id")),
                        DateTimePosted = r.GetDateTime(r.GetOrdinal("date_time_posted")),
                        Upvotes = r.GetInt32(r.GetOrdinal("upvotes")),
                        NumComments = r.GetInt32(r.GetOrdinal("num_comments")),
                        Description = r.GetString(r.GetOrdinal("description")),
                        URL = r.GetString(r.GetOrdinal("url")),
                        TopicDiscussed = r.IsDBNull(r.GetOrdinal("topic_discussed")) ? null : r.GetString(r.GetOrdinal("topic_discussed")),
                        SentimentScore = r.IsDBNull(r.GetOrdinal("sentiment_score")) ? (double?)null : r.GetDouble(r.GetOrdinal("sentiment_score"))
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }

            return redditPosts;
        }


        public void UpdatePostSentiment(RedditPost post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            if (post.SentimentScore == null)
            {
                return;
            }

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new(
                    "UPDATE reddit_posts SET sentiment_score = @sentiment_score WHERE id = @id",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("@sentiment_score", post.SentimentScore);
                cmd.Parameters.AddWithValue("@id", post.Id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }
        }

        public void UpdateTopicDiscussed(RedditPost post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            if (post.TopicDiscussed == null)
            {
                return;
            }

            _databaseService.OpenConnection();

            try
            {
                using NpgsqlCommand cmd = new(
                    "UPDATE reddit_posts SET topic_discussed = @topic_discussed WHERE id = @id",
                    _databaseService.connection);

                cmd.Parameters.AddWithValue("@topic_discussed", post.TopicDiscussed ?? string.Empty);
                cmd.Parameters.AddWithValue("@id", post.Id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _databaseService.CloseConnection();
            }
        }




    }

}
