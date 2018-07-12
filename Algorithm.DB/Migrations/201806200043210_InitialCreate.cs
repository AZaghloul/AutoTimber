namespace AutoTimber.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        InputFile = c.String(),
                        OutPutFile = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        BOQFile = c.String(),
                        Thumbnail = c.String(),
                        DesignState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Project");
        }
    }
}
