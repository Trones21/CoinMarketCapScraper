using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyScraperFirst
{
    public class Coin
    {
        [Key]
        public string PK_DateTimeandSymbol { get; set; }
        public DateTime dateTime { get; set; }
        public int Rank { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public Int64 MarketCap { get; set; }
        public bool MarketCapIsKnown { get; set; }
        public double Price{get; set;}
        public double CirculatingSupply{get; set;}
        public bool CirculatingSupplyIsKnown{get; set;}
        public bool IsMineable{get; set;}
        public double Volume24hrs{get; set;}
        public decimal PctChange_1hr{get; set;}
        public decimal PctChange_24hr{get; set;}
        public decimal PctChange_7day{get; set;}      
    }
}
