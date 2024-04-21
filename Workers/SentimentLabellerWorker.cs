using Inverse_CC_bot.Interfaces;
using System.Text.RegularExpressions;

namespace Inverse_CC_bot.Workers
{
    public class SentimentLabellerWorker : BackgroundService
    {
        private readonly ILogger<SentimentLabellerWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SentimentLabellerWorker(ILogger<SentimentLabellerWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(3000, stoppingToken); // It's jank but it works. The database refuses to connect this worker otherwise.

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting Sentiment Labeller Service");
                    using var scope = _serviceProvider.CreateScope();

                    var redditDAL = scope.ServiceProvider.GetRequiredService<IRedditDAL>();
                    var coinsDAL = scope.ServiceProvider.GetRequiredService<ICoinsDAL>();
                    var sentimentService = scope.ServiceProvider.GetRequiredService<ISentimentAnalysisService>();

                    var posts = redditDAL.GetRedditPostsWithoutSentiment();
                    var coins = coinsDAL.GetAllCoins();

                    while (posts.Count == 0)
                    {
                        _logger.LogInformation($"No Reddit Posts found, waiting for Posts Table to be populated...");
                        Thread.Sleep(3000);
                        posts = redditDAL.GetRedditPostsWithoutSentiment();
                    }

                    posts.ForEach(post =>
                    {
                        var combinedText = $"{post.PostTitle} {post.Description}";
                        var sentimentResults = sentimentService.AnalyzeSentiment(combinedText);
                        post.SentimentScore = sentimentResults.Compound;

                        var mentionedCoins = coins
                            .Where(coin =>
                                Regex.IsMatch(combinedText, $@"\b{Regex.Escape(coin.Symbol)}\b", RegexOptions.IgnoreCase) ||
                                Regex.IsMatch(combinedText, $@"\b{Regex.Escape(coin.Name)}\b", RegexOptions.IgnoreCase))
                            .Select(coin => coin.Symbol);

                        post.TopicDiscussed = string.Join(", ", mentionedCoins);

                        redditDAL.UpdatePostSentiment(post);
                        redditDAL.UpdateTopicDiscussed(post);
                    });


                    _logger.LogInformation($"Sentiment Added to {posts.Count} Posts");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while determining post sentiment");
                }

                // Adjust the delay time based on how frequently you want to run this worker.
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // RunSentiment Analysis Service every hour
            }
        }
    }
}
