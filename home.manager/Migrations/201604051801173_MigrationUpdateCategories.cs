namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationUpdateCategories : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contacts", "Category_Id", "dbo.ContactCategories");
            DropForeignKey("dbo.Notes", "Category_Id", "dbo.NoteCategories");
            DropIndex("dbo.Contacts", new[] { "Category_Id" });
            DropIndex("dbo.Notes", new[] { "Category_Id" });
            DropPrimaryKey("dbo.ContactCategories");
            DropPrimaryKey("dbo.NoteCategories");
            AlterColumn("dbo.ContactCategories", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Contacts", "Category_Id", c => c.Int());
            AlterColumn("dbo.NoteCategories", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Notes", "Category_Id", c => c.Int());
            AddPrimaryKey("dbo.ContactCategories", "Id");
            AddPrimaryKey("dbo.NoteCategories", "Id");
            CreateIndex("dbo.Contacts", "Category_Id");
            CreateIndex("dbo.Notes", "Category_Id");
            AddForeignKey("dbo.Contacts", "Category_Id", "dbo.ContactCategories", "Id");
            AddForeignKey("dbo.Notes", "Category_Id", "dbo.NoteCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "Category_Id", "dbo.NoteCategories");
            DropForeignKey("dbo.Contacts", "Category_Id", "dbo.ContactCategories");
            DropIndex("dbo.Notes", new[] { "Category_Id" });
            DropIndex("dbo.Contacts", new[] { "Category_Id" });
            DropPrimaryKey("dbo.NoteCategories");
            DropPrimaryKey("dbo.ContactCategories");
            AlterColumn("dbo.Notes", "Category_Id", c => c.Long());
            AlterColumn("dbo.NoteCategories", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Contacts", "Category_Id", c => c.Long());
            AlterColumn("dbo.ContactCategories", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.NoteCategories", "Id");
            AddPrimaryKey("dbo.ContactCategories", "Id");
            CreateIndex("dbo.Notes", "Category_Id");
            CreateIndex("dbo.Contacts", "Category_Id");
            AddForeignKey("dbo.Notes", "Category_Id", "dbo.NoteCategories", "Id");
            AddForeignKey("dbo.Contacts", "Category_Id", "dbo.ContactCategories", "Id");
        }
    }
}
