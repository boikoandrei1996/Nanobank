namespace Nanobank.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReturnedAmountFieldForDeal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deals", "ReturnedAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Deals", "ReturnedAmount");
        }
    }
}
