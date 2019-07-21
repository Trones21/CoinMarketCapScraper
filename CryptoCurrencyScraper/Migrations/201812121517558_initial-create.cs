namespace CryptoCurrencyScraperFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coins",
                c => new
                    {
                        PK_DateTimeandSymbol = c.String(nullable: false, maxLength: 128),
                        dateTime = c.DateTime(nullable: false),
                        Rank = c.Int(nullable: false),
                        Name = c.String(),
                        Symbol = c.String(),
                        MarketCap = c.Long(nullable: false),
                        MarketCapIsKnown = c.Boolean(nullable: false),
                        Price = c.Double(nullable: false),
                        CirculatingSupply = c.Double(nullable: false),
                        CirculatingSupplyIsKnown = c.Boolean(nullable: false),
                        IsMineable = c.Boolean(nullable: false),
                        Volume24hrs = c.Double(nullable: false),
                        PctChange_1hr = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PctChange_24hr = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PctChange_7day = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PK_DateTimeandSymbol);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Coins");
        }
    }
}
