namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationAddNoteCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "Category_Id", c => c.Long());
            CreateIndex("dbo.Notes", "Category_Id");
            AddForeignKey("dbo.Notes", "Category_Id", "dbo.NoteCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "Category_Id", "dbo.NoteCategories");
            DropIndex("dbo.Notes", new[] { "Category_Id" });
            DropColumn("dbo.Notes", "Category_Id");
        }
    }
}
