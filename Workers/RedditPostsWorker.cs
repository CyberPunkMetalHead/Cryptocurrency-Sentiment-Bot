using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;

namespace Inverse_CC_bot.Workers
{
    public class RedditPostsWorker : BackgroundService
    {
        private readonly ILogger<RedditPostsWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RedditPostsWorker(ILogger<RedditPostsWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting Reddit Posts Scraping Service");
                    using var scope = _serviceProvider.CreateScope();

                    var redditScrapingService = scope.ServiceProvider.GetRequiredService<IRedditScrapingService>();
                    var redditDAL = scope.ServiceProvider.GetRequiredService<IRedditDAL>();

                    List<RedditPost> redditPosts = await redditScrapingService.GetRedditPosts("cryptocurrency", 200);


                    if (redditPosts != null && redditPosts.Count > 0)
                    {
                        redditDAL.InsertRedditPosts(redditPosts);

                        _logger.LogInformation($"Inserted {redditPosts.Count} Reddit posts into the database.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching and inserting Reddit posts.");
                }

                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken); // Fetch and insert posts every 30 minutes
            }
        }
    }
}
