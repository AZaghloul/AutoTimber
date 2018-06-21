namespace Algorithm.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class again : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectVM",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Thumbnail = c.String(),
                        FileName = c.String(),
                        DesignState = c.Int(nullable: false),
                        AddedToGallery = c.Boolean(nullable: false),
                        DesignOptions_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DesignOptions", t => t.DesignOptions_Id)
                .Index(t => t.DesignOptions_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectVM", "DesignOptions_Id", "dbo.DesignOptions");
            DropIndex("dbo.ProjectVM", new[] { "DesignOptions_Id" });
            DropTable("dbo.ProjectVM");
        }
    }
}
