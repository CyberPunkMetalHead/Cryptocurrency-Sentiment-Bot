using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Inverse_CC_bot.Types;

public class RedditPostsWorker : BackgroundService
{
    private readonly ILogger<RedditPostsWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly AppConfig _config;

    public RedditPostsWorker(ILogger<RedditPostsWorker> logger, IServiceProvider serviceProvider, AppConfig config)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _config = config;
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

                List<RedditPost> redditPosts = await redditScrapingService.GetRedditPosts(_config.Subreddit, _config.Filter, _config.PostLimit);

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

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
