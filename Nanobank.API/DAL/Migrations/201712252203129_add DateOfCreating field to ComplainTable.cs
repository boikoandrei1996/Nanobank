namespace Nanobank.API.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDateOfCreatingfieldtoComplainTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Complains", "DateOfCreating", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Complains", "DateOfCreating");
        }
    }
}
