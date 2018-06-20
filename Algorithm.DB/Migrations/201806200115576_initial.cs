namespace Algorithm.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "FileName", c => c.String());
            DropColumn("dbo.Project", "InputFile");
            DropColumn("dbo.Project", "OutPutFile");
            DropColumn("dbo.Project", "BOQFile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "BOQFile", c => c.String());
            AddColumn("dbo.Project", "OutPutFile", c => c.String());
            AddColumn("dbo.Project", "InputFile", c => c.String());
            DropColumn("dbo.Project", "FileName");
        }
    }
}
