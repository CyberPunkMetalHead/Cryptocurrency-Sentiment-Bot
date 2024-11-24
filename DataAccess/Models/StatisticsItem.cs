using System.Runtime.InteropServices.JavaScript;

namespace Inverse_CC_bot.DataAccess.Models
{
    public class StatisticsItem
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public decimal Pnl { get; set; }
        
        public DateTime Date { get; set; }
    }
}