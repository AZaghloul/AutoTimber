namespace AutoTimber.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UserId");
        }
    }
}
