namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Expense : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Category_Id", c => c.Int());
            AddColumn("dbo.Expenses", "SubCategory_Id", c => c.Int());
            AlterColumn("dbo.Expenses", "Date", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Expenses", "Category_Id");
            CreateIndex("dbo.Expenses", "SubCategory_Id");
            AddForeignKey("dbo.Expenses", "Category_Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.Expenses", "SubCategory_Id", "dbo.SubCategories", "Id");
            DropColumn("dbo.Expenses", "RealDate");
            DropColumn("dbo.Expenses", "Category");
            DropColumn("dbo.Expenses", "SubCategory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "SubCategory", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "Category", c => c.Int(nullable: false));
            AddColumn("dbo.Expenses", "RealDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Expenses", "SubCategory_Id", "dbo.SubCategories");
            DropForeignKey("dbo.Expenses", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Expenses", new[] { "SubCategory_Id" });
            DropIndex("dbo.Expenses", new[] { "Category_Id" });
            AlterColumn("dbo.Expenses", "Date", c => c.Int(nullable: false));
            DropColumn("dbo.Expenses", "SubCategory_Id");
            DropColumn("dbo.Expenses", "Category_Id");
        }
    }
}
