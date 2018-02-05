namespace Nanobank.API.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReorganizeStructureOfProject : DbMigration
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
                "dbo.Complains",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Text = c.String(nullable: false, maxLength: 1000),
                        DealId = c.String(nullable: false, maxLength: 128),
                        DateOfCreating = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deals", t => t.DealId, cascadeDelete: true)
                .Index(t => t.DealId);
            
            CreateTable(
                "dbo.Deals",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 1000),
                        StartAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReturnedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FCMPushNotificationToken = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Deals", "UserOwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Deals", "UserCreditorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserInfoes", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserInfoes", "CardNumber", "dbo.CreditCards");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Complains", "DealId", "dbo.Deals");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.UserInfoes", new[] { "CardNumber" });
            DropIndex("dbo.UserInfoes", "IX_UniqueFullName");
            DropIndex("dbo.UserInfoes", new[] { "Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Deals", new[] { "UserCreditorId" });
            DropIndex("dbo.Deals", new[] { "UserOwnerId" });
            DropIndex("dbo.Complains", new[] { "DealId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Deals");
            DropTable("dbo.Complains");
            DropTable("dbo.CreditCards");
        }
    }
}
