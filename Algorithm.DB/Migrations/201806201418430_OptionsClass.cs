namespace AutoTimber.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionsClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DesignOptions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        FrameWalls = c.Int(nullable: false),
                        FrameFloors = c.Int(nullable: false),
                        FrameRafter = c.Int(nullable: false),
                        DetectExternalWalls = c.Int(nullable: false),
                        OptimizeWalls = c.Int(nullable: false),
                        OptimizeFloors = c.Int(nullable: false),
                        OptimizeRafter = c.Int(nullable: false),
                        Exclude = c.Int(nullable: false),
                        DeleteArcWalls = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Project", "AddToGallery", c => c.Boolean(nullable: false));
            AddColumn("dbo.Project", "DesignOptions_Id", c => c.Guid());
            CreateIndex("dbo.Project", "DesignOptions_Id");
            AddForeignKey("dbo.Project", "DesignOptions_Id", "dbo.DesignOptions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Project", "DesignOptions_Id", "dbo.DesignOptions");
            DropIndex("dbo.Project", new[] { "DesignOptions_Id" });
            DropColumn("dbo.Project", "DesignOptions_Id");
            DropColumn("dbo.Project", "AddToGallery");
            DropTable("dbo.DesignOptions");
        }
    }
}
