using Inverse_CC_bot.DataAccess.Repositories;
using Inverse_CC_bot.Interfaces;
using Inverse_CC_bot.Services;
using Inverse_CC_bot.Types;
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

                   //Store the general Configuration Settings that determine the behaviour of the application
                   var appConfig = hostContext.Configuration.GetSection("AppConfig").Get<AppConfig>();

                   if (appConfig == null) { return; }


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
                   //services.AddHostedService<ExchangeBuyWorker>();

                   services.AddHostedService(provider =>
                   {
                       var logger = provider.GetRequiredService<ILogger<ExchangeBuyWorker>>();
                       var serviceProvider = provider.GetRequiredService<IServiceProvider>();
                       return new ExchangeBuyWorker(logger, serviceProvider, appConfig);
                   });


                   // TO DO - Create a Portfolio Manager Worker that calculates PNL and Sells coins.


               })
               .Build();

host.Run();
