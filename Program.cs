using Inverse_CC_bot.DataAccess.Repositories;
using Inverse_CC_bot.Interfaces;
using Inverse_CC_bot.Services;
using Inverse_CC_bot.Workers;

IHost host = Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config.SetBasePath(AppContext.BaseDirectory);
                   config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
               })
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddScoped<IRedditScrapingService, RedditScrapingService>();

                   // Retrieve the connection string and register the db
                   string connectionString = hostContext.Configuration.GetConnectionString("Database");
                   string apiKey = hostContext.Configuration.GetSection("ApiConfig:ApiKey").Value;
                   string apiSecret = hostContext.Configuration.GetSection("ApiConfig:ApiSecret").Value;


                   services.AddScoped(_ => new DatabaseService(connectionString));
                   services.AddScoped<IExchangeService>(_ => new ExchangeService(Inverse_CC_bot.Enums.ExchangeNameEnum.Binance, apiKey, apiSecret));


                   services.AddScoped<IRedditDAL, RedditDAL>();
                   services.AddScoped<ICoinsDAL, CoinsDAL>();
                   services.AddScoped<ICoinSentimentsDAL, CoinSentimentsDAL>();
                   services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();
                   services.AddScoped<ISentimentAggregatorService, SentimentAggregatorService>();
                   services.AddScoped<IOrdersDAL, OrdersDAL>();
                   services.AddScoped<IPortfolioDAL, PortfolioDAL>();

                   services.AddHostedService<RedditPostsWorker>();
                   services.AddHostedService<SentimentLabellerWorker>();
                   services.AddHostedService<SentimentAggregatorWorker>();
                   services.AddHostedService<ExchangeBuyWorker>();

                   // TO DO - Create a Portfolio Manager Worker that calculates PNL and Sells coins.


               })
               .Build();

host.Run();
