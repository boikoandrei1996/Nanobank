namespace Nanobank.API.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcomplaintable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Complains",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Text = c.String(nullable: false, maxLength: 1000),
                        DealId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deals", t => t.DealId, cascadeDelete: true)
                .Index(t => t.DealId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Complains", "DealId", "dbo.Deals");
            DropIndex("dbo.Complains", new[] { "DealId" });
            DropTable("dbo.Complains");
        }
    }
}
