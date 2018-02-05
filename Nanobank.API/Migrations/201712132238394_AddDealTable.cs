namespace Nanobank.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDealTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deals",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 1000),
                        StartAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DealDurationInMonth = c.Short(nullable: false),
                        PercentRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserOwnerId = c.String(nullable: false, maxLength: 128),
                        UserCreditorId = c.String(maxLength: 128),
                        DealStartDate = c.DateTime(),
                        RatingPositive = c.Short(),
                        RatingNegative = c.Short(),
                        DealClosedDate = c.DateTime(),
                        IsClosed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserCreditorId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserOwnerId, cascadeDelete: true)
                .Index(t => t.UserOwnerId)
                .Index(t => t.UserCreditorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deals", "UserOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Deals", "UserCreditorId", "dbo.AspNetUsers");
            DropIndex("dbo.Deals", new[] { "UserCreditorId" });
            DropIndex("dbo.Deals", new[] { "UserOwnerId" });
            DropTable("dbo.Deals");
        }
    }
}
