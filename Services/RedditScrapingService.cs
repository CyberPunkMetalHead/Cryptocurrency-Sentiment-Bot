using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using System.Text.Json;


namespace Inverse_CC_bot.Services
{
    public class RedditScrapingService : IRedditScrapingService
    {
        private readonly HttpClient _httpClient;
        private const string RedditBaseUrl = "https://www.reddit.com";

        public RedditScrapingService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

        }

        // TODO: Add pagination to get over the 100 hardcap limit
        public async Task<List<RedditPost>> GetRedditPosts(string subreddit, int count)
        {
            try
            {
                string url = $"{RedditBaseUrl}/r/{subreddit}/hot.json?limit={count}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode) { return null; }

                using var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var redditDataResponse = await JsonSerializer.DeserializeAsync<RedditPageResponse>(responseStream, options);
                var posts = new List<RedditPost>();

                foreach (var postListing in redditDataResponse.Data.Children)
                {
                    var post = postListing.Data;
                    posts.Add(new RedditPost
                    {
                        PostId = post.Id,
                        PostTitle = post.Title,
                        Description = post.SelfText,
                        Upvotes = post.Upvotes,
                        NumComments = post.NumComments,
                        DateTimePosted = post.CreatedUtc
                        // You can add more properties as needed
                    });
                }
                return posts;
            }

            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

    }
}
