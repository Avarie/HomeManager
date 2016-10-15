namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubCategories", "Category_Id", c => c.Int());
            CreateIndex("dbo.SubCategories", "Category_Id");
            AddForeignKey("dbo.SubCategories", "Category_Id", "dbo.Categories", "Id");
            DropColumn("dbo.SubCategories", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubCategories", "CategoryId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SubCategories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.SubCategories", new[] { "Category_Id" });
            DropColumn("dbo.SubCategories", "Category_Id");
        }
    }
}
