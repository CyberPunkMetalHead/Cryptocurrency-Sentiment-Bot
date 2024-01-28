using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inverse_CC_bot.DataAccess.Models
{
    public class Coin
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Symbol { get; set; } = "";
    }
}
