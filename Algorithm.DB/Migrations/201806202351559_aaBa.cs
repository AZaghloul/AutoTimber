namespace AutoTimber.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaBa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectVM", "DesignOptions_Id", "dbo.DesignOptions");
            DropIndex("dbo.ProjectVM", new[] { "DesignOptions_Id" });
            DropTable("dbo.ProjectVM");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ProjectVM", "DesignOptions_Id");
            AddForeignKey("dbo.ProjectVM", "DesignOptions_Id", "dbo.DesignOptions", "Id");
        }
    }
}
