namespace Inverse_CC_bot.Types
{
    public class AppConfig
    {
        public bool PaperTrading { get; set; }
        public int Amount { get; set; }
        public decimal SentimentThreshold { get; set; }
        public string BaseCurrency { get; set; } = string.Empty;
        public string Subreddit { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public int PostLimit { get; set; } = 0;
    }

}
