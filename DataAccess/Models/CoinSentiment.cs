namespace Inverse_CC_bot.DataAccess.Models
{
    public class CoinSentiment
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = "";
        public DateTime Date { get; set; }
        public double SentimentValue { get; set; }
    }
}
