namespace Algorithm.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WoodSetup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Project", "DesignOptions_Id", "dbo.DesignOptions");
            RenameColumn(table: "dbo.Project", name: "DesignOptions_Id", newName: "DesignOptions_ProjectId");
            RenameIndex(table: "dbo.Project", name: "IX_DesignOptions_Id", newName: "IX_DesignOptions_ProjectId");
            DropPrimaryKey("dbo.DesignOptions");
            CreateTable(
                "dbo.WoodSetup",
                c => new
                    {
                        ProjectId = c.Guid(nullable: false),
                        HeaderSection = c.Int(nullable: false),
                        StudSection = c.Int(nullable: false),
                        JoistSection = c.Int(nullable: false),
                        WoodGrade = c.Int(nullable: false),
                        WoodSpecies = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            AddColumn("dbo.Project", "WoodSetup_ProjectId", c => c.Guid());
            AddPrimaryKey("dbo.DesignOptions", "ProjectId");
            CreateIndex("dbo.Project", "WoodSetup_ProjectId");
            AddForeignKey("dbo.Project", "WoodSetup_ProjectId", "dbo.WoodSetup", "ProjectId");
            AddForeignKey("dbo.Project", "DesignOptions_ProjectId", "dbo.DesignOptions", "ProjectId");
            DropColumn("dbo.DesignOptions", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DesignOptions", "Id", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Project", "DesignOptions_ProjectId", "dbo.DesignOptions");
            DropForeignKey("dbo.Project", "WoodSetup_ProjectId", "dbo.WoodSetup");
            DropIndex("dbo.Project", new[] { "WoodSetup_ProjectId" });
            DropPrimaryKey("dbo.DesignOptions");
            DropColumn("dbo.Project", "WoodSetup_ProjectId");
            DropTable("dbo.WoodSetup");
            AddPrimaryKey("dbo.DesignOptions", "Id");
            RenameIndex(table: "dbo.Project", name: "IX_DesignOptions_ProjectId", newName: "IX_DesignOptions_Id");
            RenameColumn(table: "dbo.Project", name: "DesignOptions_ProjectId", newName: "DesignOptions_Id");
            AddForeignKey("dbo.Project", "DesignOptions_Id", "dbo.DesignOptions", "Id");
        }
    }
}
