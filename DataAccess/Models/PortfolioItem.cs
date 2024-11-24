
namespace Inverse_CC_bot.DataAccess.Models
{
    public class PortfolioItem
    {
        public int Id { get; set; }
        public required string OrderId { get; set; }
        
        public required string Symbol { get; set; }
        public decimal? Pnl { get; set; }
    }
}
