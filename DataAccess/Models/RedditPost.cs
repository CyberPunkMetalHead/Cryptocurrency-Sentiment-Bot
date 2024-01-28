
namespace Inverse_CC_bot.DataAccess.Models
{
    public class RedditPost
    {
        public int Id { get; set; }
        public string PostId { get; set; } = "";
        public string PostTitle { get; set; } = "";
        public DateTime DateTimePosted { get; set; }
        public int Upvotes { get; set; }
        public int NumComments { get; set; }
        public string Description { get; set; } = "";
        public string URL { get; set; } = "";
        public string? TopicDiscussed { get; set; }
        public double? SentimentScore { get; set; }
    }
}
