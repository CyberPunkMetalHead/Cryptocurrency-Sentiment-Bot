using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Inverse_CC_bot.Services;


namespace Inverse_CC_bot.Workers
{
    public class ExchangeBuyWorker : BackgroundService
    {
        private readonly ILogger<ExchangeBuyWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ExchangeBuyWorker(ILogger<ExchangeBuyWorker> logger, IServiceProvider serviceProvider)
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
                    _logger.LogInformation("Starting Exchange Buy Service");
                    using var scope = _serviceProvider.CreateScope();

                    var exchangeService = scope.ServiceProvider.GetRequiredService<IExchangeService>();
                    var coinSentimentsDAL = scope.ServiceProvider.GetRequiredService<ICoinSentimentsDAL>();
                    var portfolioDAL = scope.ServiceProvider.GetRequiredService<IPortfolioDAL>();
                    var ordersDAL = scope.ServiceProvider.GetRequiredService<IOrdersDAL>();


                    List<CoinSentiment> coinSentiments = coinSentimentsDAL.GetAllCoinSentiments();

                    while (coinSentiments.Count == 0)
                    {
                        _logger.LogInformation($"Exchange Worker could not find any coins in the Coin Sentiments Table. Trying again in 3s...");
                        Thread.Sleep(3000);
                        coinSentiments = coinSentimentsDAL.GetAllCoinSentiments();
                    }

                    coinSentiments.ForEach(async coin =>
                    {
                        if (coin.SentimentValue < 0.1)
                        {
                            _logger.LogInformation($"Attempting to place order for {coin.Symbol + "-USDT"}");

                            var order = await exchangeService.PlaceOrder(coin.Symbol + "-USDT", 30);
                            if (order != null)
                            {

                                _logger.LogInformation($"Order Placed for {coin.Symbol}");

                                ordersDAL.InsertOrder(order);
                                portfolioDAL.InsertPortfolioItem(new PortfolioItem
                                {
                                    OrderId = order.OrderId
                                });

                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred in the Exchange Worker: {ex}");
                }
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Buy Coins with negative sentiment Every day
            }
        }
    }
}
