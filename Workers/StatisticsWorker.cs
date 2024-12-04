using ExchangeSharp;
using Inverse_CC_bot.DataAccess.Models;
using Inverse_CC_bot.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inverse_CC_bot.Services;

namespace Interactive_CC_bot.Workers
{
    public class StatisticsWorker : BackgroundService
    {
        private readonly ILogger<StatisticsWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public StatisticsWorker(ILogger<StatisticsWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(3000, stoppingToken); // Initial delay before starting

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting Statistics Worker");

                    using var scope = _serviceProvider.CreateScope();

                    var portfolioDAL = scope.ServiceProvider.GetRequiredService<IPortfolioDAL>();
                    var ordersDAL = scope.ServiceProvider.GetRequiredService<IOrdersDAL>();
                    var statisticsDAL = scope.ServiceProvider.GetRequiredService<IStatisticsDAL>();
                    var exchangeService = scope.ServiceProvider.GetRequiredService<IExchangeService>();
                    
                    List<PortfolioItem> portfolio = portfolioDAL.GetAllPortfolioItems();
                    
                    foreach (var portfolioItem in portfolio)
                    {
                        try
                        {
                            var orders = ordersDAL.GetOrdersBySymbol(portfolioItem.Symbol);
                            var averageBuy = orders.Average(o => o.Price);
                            var currentPrice = await exchangeService.GetPrice(portfolioItem.Symbol+"-USDT");
                            var pnl = (currentPrice - averageBuy) / averageBuy * 100 ?? 0;
                            var newStatistic = new StatisticsItem
                            {
                                Symbol = portfolioItem.Symbol,
                                Pnl = pnl,
                                Date = new DateTime()
                            };
                            
                            portfolioDAL.UpdatePortfolioPnlById(portfolioItem.Id, pnl);
                            statisticsDAL.InsertStatisticsItem(newStatistic);
                        
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(
                                $"Could not calculate PNL for: {portfolioItem.Symbol}: {ex.Message}");
                        }
                    }

                    _logger.LogInformation("Completed statistics update for all portfolio items");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while calculating and updating statistics");
                }

                // Delay next execution
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Run the statistics update every hour
            }
        }
    }
}
