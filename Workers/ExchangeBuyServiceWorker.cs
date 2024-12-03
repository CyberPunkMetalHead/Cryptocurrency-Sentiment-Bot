using ExchangeSharp;
using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Inverse_CC_bot.Services;
using Inverse_CC_bot.Types;


namespace Inverse_CC_bot.Workers
{
    public class ExchangeBuyWorker : BackgroundService
    {
        private readonly ILogger<ExchangeBuyWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly AppConfig _config;


        public ExchangeBuyWorker(ILogger<ExchangeBuyWorker> logger, IServiceProvider serviceProvider, AppConfig config)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _config = config;
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

                    // Handles Placing PAPER and LIVE orders
                    coinSentiments.ForEach(coin =>
                    {
                        if (!(coin.SentimentValue < 0.1)) return;
                        
                        ExchangeOrderResult? order = null;
                        var orderType = _config.PaperTrading ? "PAPER" : "LIVE";

                        _logger.LogInformation($"Trying to place {orderType} order for {coin.Symbol + "USDT"}");
                        try
                        {
                            order = _config.PaperTrading ? exchangeService.PlacePaperOrder(coin.Symbol + "USDT", _config.Amount).Result :
                            exchangeService.PlaceOrder(coin.Symbol + "USDT", _config.Amount).Result;

                            _logger.LogInformation($"{orderType} Order Placed for {coin.Symbol}: {order}");

                            if (order == null) return;
                            
                            ordersDAL.InsertOrder(order);
                            portfolioDAL.InsertPortfolioItem(new PortfolioItem
                            {
                                OrderId = order.OrderId,
                                Symbol = coin.Symbol
                            });
                            
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error placing {orderType} order for {coin.Symbol}USDT. The coin probably doesn't exist: {ex}");
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
