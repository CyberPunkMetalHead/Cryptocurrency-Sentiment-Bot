using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;

namespace Inverse_CC_bot.Workers
{
    public class SentimentAggregatorWorker : BackgroundService
    {
        private readonly ILogger<SentimentAggregatorWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SentimentAggregatorWorker(ILogger<SentimentAggregatorWorker> logger, IServiceProvider serviceProvider)
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
                    _logger.LogInformation("Starting Sentiment Aggregator Service");
                    using var scope = _serviceProvider.CreateScope();

                    var redditDAL = scope.ServiceProvider.GetRequiredService<IRedditDAL>();
                    var coinsDAL = scope.ServiceProvider.GetRequiredService<ICoinsDAL>();
                    var coinSentimentsDAL = scope.ServiceProvider.GetRequiredService<ICoinSentimentsDAL>();
                    var sentimentAggregatorService = scope.ServiceProvider.GetRequiredService<ISentimentAggregatorService>();

                    var posts = redditDAL.GetRedditPostsWithSentiment();


                    while (posts.Count == 0)
                    {
                        _logger.LogInformation($"Sentiment Aggregator could not find any Reddit posts with a Sentiment. Trying again in 3s...");
                        Thread.Sleep(3000);
                        posts = redditDAL.GetRedditPostsWithSentiment();
                    }

                    var filteredPosts = posts.Where(post => !string.IsNullOrEmpty(post.TopicDiscussed)).ToList();

                    var aggregatedSentiments = sentimentAggregatorService.AggregateSentiments(filteredPosts);

                    foreach (var coin in aggregatedSentiments.Keys)
                    {
                        double averageSentiment = sentimentAggregatorService.CalculateAverageSentiment(aggregatedSentiments[coin]);

                        coinSentimentsDAL.InsertCoinSentiment(new CoinSentiment
                        {
                            Symbol = coin,
                            Date = DateTime.Today,
                            SentimentValue = averageSentiment
                        });
                    }

                    _logger.LogInformation($"Sentiment Average Added For {aggregatedSentiments.Count} Coins");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while aggregating and adding coin sentiments.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
