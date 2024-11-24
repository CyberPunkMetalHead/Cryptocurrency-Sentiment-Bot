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

                    // Fetch orders and portfolio items
                    List<ExchangeOrderResult> orders = ordersDAL.GetAllOrders();
                    List<PortfolioItem> portfolio = portfolioDAL.GetAllPortfolioItems();

                    // Dictionary to hold aggregated PNL by symbol
                    var symbolPNLMap = new Dictionary<string, decimal>();
                    var symbolCountMap = new Dictionary<string, int>();

                    // Step 1: Calculate PNL for each portfolio item
                    foreach (var portfolioItem in portfolio)
                    {
                        // Get current price from exchange service
                        decimal currentPrice = await exchangeService.GetPrice(portfolioItem.OrderId);

                        // Calculate the PNL for the item (assuming it is price-based)
                        decimal pnl = (portfolioItem.Pnl ?? 0) + ((currentPrice - portfolioItem.Pnl ?? 0) / portfolioItem.Pnl ?? 0) * 100;

                        // Aggregate by symbol
                        if (symbolPNLMap.ContainsKey(portfolioItem.Symbol))
                        {
                            symbolPNLMap[portfolioItem.Symbol] += pnl;
                            symbolCountMap[portfolioItem.Symbol]++;
                        }
                        else
                        {
                            symbolPNLMap[portfolioItem.Symbol] = pnl;
                            symbolCountMap[portfolioItem.Symbol] = 1;
                        }
                    }

                    // Step 2: Calculate average PNL for each symbol and insert or update statistics
                    foreach (var symbol in symbolPNLMap.Keys.ToList())
                    {
                        decimal averagePnl = symbolPNLMap[symbol] / symbolCountMap[symbol];

                        // Check if the symbol exists in the database and update or insert accordingly
                        try
                        {
                            // Check if the statistic exists for the symbol
                            var existingStatistic = statisticsDAL.GetStatisticsBySymbol(symbol);

                            if (existingStatistic != null)
                            {
                                // If symbol and date exist, update its PNL
                                statisticsDAL.UpdateStatisticsItemBySymbol(symbol, averagePnl, new DateTime());
                            }
                            else
                            {
                                // If symbol and date do not exist, insert a new entry
                                var newStatistic = new StatisticsItem
                                {
                                    Symbol = symbol,
                                    Pnl = averagePnl,
                                    Date = new DateTime()
                                };
                                statisticsDAL.InsertStatisticsItem(newStatistic);
                            }

                            _logger.LogInformation($"Statistics updated for symbol: {symbol} with PNL: {averagePnl}");

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error updating/inserting statistics for symbol: {symbol}");
                        }
                    }

                    _logger.LogInformation("Completed statistics update for all portfolio items");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while calculating and updating statistics");
                }

                // Delay next execution
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Run the statistics update every hour
            }
        }
    }
}
