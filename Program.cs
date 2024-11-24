using Interactive_CC_bot.Workers;
using Inverse_CC_bot.DataAccess.Repositories;
using Inverse_CC_bot.Interfaces;
using Inverse_CC_bot.Services;
using Inverse_CC_bot.Types;
using Inverse_CC_bot.Workers;
using Microsoft.Extensions.Options;

IHost host = Host.CreateDefaultBuilder(args)
   .ConfigureAppConfiguration((hostingContext, config) =>
   {
       config.SetBasePath(AppContext.BaseDirectory);
       config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
   })
   .ConfigureServices((hostContext, services) =>
   {
       // Register AppConfig as a singleton, bound from appsettings.json
       services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));
       services.AddSingleton(sp => sp.GetRequiredService<IOptions<AppConfig>>().Value);

       // Register other services
       services.AddScoped<IRedditScrapingService, RedditScrapingService>();

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
       services.AddScoped<IStatisticsDAL, StatisticsDAL>();


       // Register RedditPostsWorker and other workers
       services.AddHostedService<RedditPostsWorker>();
       services.AddHostedService<SentimentLabellerWorker>();
       services.AddHostedService<SentimentAggregatorWorker>();
       services.AddHostedService<StatisticsWorker>();

       services.AddHostedService(provider =>
       {
           var logger = provider.GetRequiredService<ILogger<ExchangeBuyWorker>>();
           var serviceProvider = provider.GetRequiredService<IServiceProvider>();
           var appConfig = provider.GetRequiredService<AppConfig>();
           return new ExchangeBuyWorker(logger, serviceProvider, appConfig);
       });
   })
   .Build();

host.Run();
