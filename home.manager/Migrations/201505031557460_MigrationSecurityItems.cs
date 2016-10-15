namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationSecurityItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SecurityCategories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SecurityItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Link = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        Date = c.DateTime(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Category_Id = c.Long(),
                        Owner_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SecurityCategories", t => t.Category_Id)
                .ForeignKey("dbo.UserProfile", t => t.Owner_UserId)
                .Index(t => t.Category_Id)
                .Index(t => t.Owner_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SecurityItems", "Owner_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.SecurityItems", "Category_Id", "dbo.SecurityCategories");
            DropIndex("dbo.SecurityItems", new[] { "Owner_UserId" });
            DropIndex("dbo.SecurityItems", new[] { "Category_Id" });
            DropTable("dbo.SecurityItems");
            DropTable("dbo.SecurityCategories");
        }
    }
}
