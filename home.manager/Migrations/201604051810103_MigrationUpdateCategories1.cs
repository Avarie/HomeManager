namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationUpdateCategories1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SecurityItems", "Category_Id", "dbo.SecurityCategories");
            DropIndex("dbo.SecurityItems", new[] { "Category_Id" });
            DropPrimaryKey("dbo.SecurityCategories");
            AlterColumn("dbo.SecurityCategories", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SecurityItems", "Category_Id", c => c.Int());
            AddPrimaryKey("dbo.SecurityCategories", "Id");
            CreateIndex("dbo.SecurityItems", "Category_Id");
            AddForeignKey("dbo.SecurityItems", "Category_Id", "dbo.SecurityCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SecurityItems", "Category_Id", "dbo.SecurityCategories");
            DropIndex("dbo.SecurityItems", new[] { "Category_Id" });
            DropPrimaryKey("dbo.SecurityCategories");
            AlterColumn("dbo.SecurityItems", "Category_Id", c => c.Long());
            AlterColumn("dbo.SecurityCategories", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.SecurityCategories", "Id");
            CreateIndex("dbo.SecurityItems", "Category_Id");
            AddForeignKey("dbo.SecurityItems", "Category_Id", "dbo.SecurityCategories", "Id");
        }
    }
}
