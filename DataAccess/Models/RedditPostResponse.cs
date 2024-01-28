
namespace Inverse_CC_bot.DataAccess.Models
{
    public class RedditPageResponse
    {
        public RedditData? Data { get; set; }
    }

    public class RedditData
    {
        public List<RedditPostListing> Children { get; set; } = new List<RedditPostListing>();
    }

    public class RedditPostListing
    {
        public RedditPostResponse Data { get; set; }
    }

    public class RedditPostResponse
    {
        public string Id { get; set; } = "";
        public string Author { get; set; } = "";
        public string Title { get; set; } = "";
        public string SelfText { get; set; } = "";
        public int Upvotes { get; set; }
        public int NumComments { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
