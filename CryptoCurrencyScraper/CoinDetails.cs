using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoCurrencyScraperFirst
{
    class CoinDetails
    {
        [Key]
        public string Symbol { get; set; }
        public string TradingLink { get; set; }

        [Column(TypeName="Date")]
        public DateTime DateAdded { get; set; }
        //This can be done via the historical data page as well
        
        //List of Exchanges?
    }
}
