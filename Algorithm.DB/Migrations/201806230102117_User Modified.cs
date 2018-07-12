namespace AutoTimber.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "FirstName", c => c.String());
            AddColumn("dbo.User", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "LastName");
            DropColumn("dbo.User", "FirstName");
        }
    }
}
