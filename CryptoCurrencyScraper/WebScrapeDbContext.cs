using System.Data.Entity;

namespace CryptoCurrencyScraperFirst
{
    public class WebScrapeDbContext : DbContext
    {
        public DbSet<Coin> DbSet_Coin {get; set;}

    }
}
