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
        public async Task<List<RedditPost>> GetRedditPosts(string subreddit, string filter, int count)
        {
            try
            {
                const int batchSize = 100; // Number of posts to fetch per request
                var posts = new List<RedditPost>();
                string after = null;

                while (count > 0)
                {
                    int fetchCount = Math.Min(count, batchSize);
                    string url = $"{RedditBaseUrl}/r/{subreddit}/{filter}.json?limit={fetchCount}";

                    if (after != null)
                    {
                        url += $"&after={after}";
                    }

                    HttpResponseMessage response = await _httpClient.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        break;
                    }

                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var redditDataResponse = await JsonSerializer.DeserializeAsync<RedditPageResponse>(responseStream, options);

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

                    // Update the 'after' parameter for the next request
                    after = redditDataResponse.Data.Children.LastOrDefault()?.Data.Name;
                    count -= batchSize;
                }

                return posts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

    }
}
