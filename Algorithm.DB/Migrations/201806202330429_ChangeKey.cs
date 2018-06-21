namespace Algorithm.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.User");
            AlterColumn("dbo.Project", "UserId", c => c.String());
            AlterColumn("dbo.User", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.User");
            AlterColumn("dbo.User", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Project", "UserId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.User", "Id");
        }
    }
}
