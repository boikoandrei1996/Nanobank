namespace Nanobank.API.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PushNotificationToken_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FCMPushNotificationToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FCMPushNotificationToken");
        }
    }
}
