namespace Nanobank.API.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserInfoandCreditCardtables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        CardNumber = c.String(nullable: false, maxLength: 128),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CardNumber);
            
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Patronymic = c.String(nullable: false, maxLength: 100),
                        CardNumber = c.String(nullable: false, maxLength: 128),
                        CardDateOfExpire = c.DateTime(nullable: false),
                        CardOwnerFullName = c.String(nullable: false, maxLength: 255),
                        CardCVV2Key = c.String(nullable: false, maxLength: 3),
                        PassportImage = c.Binary(nullable: false, storeType: "image"),
                        ImageMimeType = c.String(nullable: false, maxLength: 10),
                        RatingPositive = c.Long(),
                        RatingNegative = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CreditCards", t => t.CardNumber, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id)
                .Index(t => new { t.FirstName, t.LastName, t.Patronymic }, unique: true, name: "IX_UniqueFullName")
                .Index(t => t.CardNumber);
            
            AddColumn("dbo.AspNetUsers", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInfoes", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserInfoes", "CardNumber", "dbo.CreditCards");
            DropIndex("dbo.UserInfoes", new[] { "CardNumber" });
            DropIndex("dbo.UserInfoes", "IX_UniqueFullName");
            DropIndex("dbo.UserInfoes", new[] { "Id" });
            DropColumn("dbo.AspNetUsers", "IsApproved");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.CreditCards");
        }
    }
}
