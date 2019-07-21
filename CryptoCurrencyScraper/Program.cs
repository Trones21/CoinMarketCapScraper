using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace CryptoCurrencyScraperFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            //var test = new EmailSender();
            //test.SendMail("Test Email: " + DateTime.Now, "Testing my Email Server");

            //using(var context = new WebScrapeDbContext())
            //{
            //    context.Database.Delete();
            //    context.Database.Create();
            //}

            var coinscrape = new CoinScraper();
            coinscrape.Start();

            if (coinscrape.InsertCoins())
            {
                //Get the number of Coins and SendMail
                var queryContext = new WebScrapeDbContext();
                var newrecords = queryContext.DbSet_Coin
                                .Where(c => c.dateTime == coinscrape.DateTime_onSite)
                                .Select(c => c.Name)
                                .ToList();
                var count = newrecords.Count();
                var SuccessEmailer = new EmailSender();
                SuccessEmailer.SendMail("Crypto DB Updated: " + DateTime.Now, "Records inserted: " + count.ToString());
            }

        }

        public class CoinScraper : WebScraper
        {
            List<Coin> Coins = new List<Coin>();
            public DateTime DateTime_onSite;
            public override void Init()
            {
                this.HttpTimeOut = TimeSpan.FromSeconds(20000);

                this.Request("https://coinmarketcap.com/all/views/all/", Parse);
                
            }

            public override void Parse(Response response)
            {
                var lastupdate_onSite_str = response.QuerySelector(".text-center.text-gray").InnerText;
                lastupdate_onSite_str = lastupdate_onSite_str.Replace("Last updated: ", "").Replace(" UTC", "");
                var lastupdate_onSite = DateTime.Parse(lastupdate_onSite_str);
                DateTime_onSite = lastupdate_onSite;

                var lastupdate_inDB = new DateTime();
                lastupdate_inDB = GetLastUpdatefromDB();
               
                if (lastupdate_inDB == DateTime_onSite)
                {
                    var EmailSender = new EmailSender();
                    EmailSender.SendMail("Site has not been updated yet", "Last update in DB: " + lastupdate_inDB + "//n Last Update on Site: " + lastupdate_onSite);
                    System.Environment.Exit(0);
                }
                else
                {
                    this.Request("https://coinmarketcap.com/all/views/all/", ScrapeCoins);                   
                }
            }
            public DateTime GetLastUpdatefromDB()
            {
                using (var db = new WebScrapeDbContext())
                {
                    
                    var maxdateinDB = from coin in db.DbSet_Coin
                                      orderby coin.dateTime descending
                                      select coin.dateTime;
                    return maxdateinDB.FirstOrDefault();
                 }
            }
                          

            public void ScrapeCoins(Response response)
            {
                var coins = response.QuerySelectorAll("#currencies-all > tbody > tr");

                foreach (var coin in coins)
                {
                    var newCoin = new Coin();
                    newCoin.dateTime = DateTime_onSite;
                    newCoin.Rank = Convert.ToInt32(coin.QuerySelector(".text-center").InnerText);
                    //PK_Udemy_Course_Refactor/Debug - SHow how using context.database.log = console.writeline can help identify the error
                    newCoin.Name = coin.QuerySelector(".no-wrap.currency-name").GetAttribute("data-sort");
                    newCoin.Symbol = coin.QuerySelector(".text-left.col-symbol").InnerText;


                    if (!coin.QuerySelector(".no-wrap.market-cap.text-right").GetAttribute("data-usd").Contains("?"))
                    {
                        newCoin.MarketCapIsKnown = true;
                        newCoin.MarketCap = Int64.Parse(coin.QuerySelector(".no-wrap.market-cap.text-right").InnerText.Replace("$", "").Replace(",", ""));
                    }
                    else
                    {
                        newCoin.MarketCapIsKnown = false;
                    }
                    newCoin.Price = float.Parse(coin.QuerySelectorAll(".no-wrap.text-right")[1].GetAttribute("data-sort"));

                    if (coin.QuerySelector(".circulating-supply").InnerText.Contains("*"))
                    {
                        newCoin.IsMineable = false;
                    }
                    else
                    {
                        newCoin.IsMineable = true;
                    }


                    if (!coin.QuerySelector(".circulating-supply").InnerText.Contains("?"))
                    {
                        newCoin.CirculatingSupply = double.Parse(coin.QuerySelector(".circulating-supply").GetAttribute("data-sort"));
                        newCoin.CirculatingSupplyIsKnown = true;
                    }
                    else
                    {
                        newCoin.CirculatingSupplyIsKnown = false;
                    }

                    //PK_Udemy_Course - Show the original versions and then show how to fix it
                    //Solution 1 - Try Parse
                    //Solution 2 - Try Parse Method, make generic?
                    var parsedDouble = new double();
                    if (double.TryParse(coin.QuerySelector(".volume").GetAttribute("data-usd"), out parsedDouble))
                    {
                        newCoin.Volume24hrs = parsedDouble;
                    }

                    var parsedDecimal = new Decimal();
                    if (decimal.TryParse(coin.QuerySelectorAll(".text-right")[4].GetAttribute("data-percentusd"), out parsedDecimal))
                    {
                        newCoin.PctChange_1hr = parsedDecimal;
                    }
                    if (decimal.TryParse(coin.QuerySelectorAll(".text-right")[5].GetAttribute("data-percentusd"), out parsedDecimal))
                    {
                        newCoin.PctChange_24hr = parsedDecimal;
                    }
                    if (decimal.TryParse(coin.QuerySelectorAll(".text-right")[6].GetAttribute("data-percentusd"), out parsedDecimal))
                    {
                        newCoin.PctChange_7day = parsedDecimal;
                    }

                    newCoin.PK_DateTimeandSymbol = newCoin.Symbol + "_" + newCoin.Name + "_" + newCoin.dateTime.ToString();
                    Coins.Add(newCoin);
                }
            }
            public bool InsertCoins()
            {
                using (var context = new WebScrapeDbContext())
                {
                    var log = context.Database.Log;
                    try
                    {
                        //context.Database.Log = Console.WriteLine;
                        context.DbSet_Coin.AddRange(Coins);
                        context.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //PK_Udemy show how to Iterate development                        
                        Console.ReadKey();
                        EmailSender sender = new EmailSender();
                        sender.SendMail("DB Insert Exception", ex.Message + "/n" + log.ToString());
                        return false;
                    }
                }

            }

        }
    }
    
}
