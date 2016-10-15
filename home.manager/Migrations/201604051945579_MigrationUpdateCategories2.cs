namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationUpdateCategories2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContactLines", "Contact_Id", "dbo.Contacts");
            DropIndex("dbo.ContactLines", new[] { "Contact_Id" });
            DropPrimaryKey("dbo.Contacts");
            DropPrimaryKey("dbo.SecurityItems");
            AddColumn("dbo.SecurityItems", "Name", c => c.String());
            AlterColumn("dbo.ContactLines", "Contact_Id", c => c.Int());
            AlterColumn("dbo.Contacts", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SecurityItems", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Contacts", "Id");
            AddPrimaryKey("dbo.SecurityItems", "Id");
            CreateIndex("dbo.ContactLines", "Contact_Id");
            AddForeignKey("dbo.ContactLines", "Contact_Id", "dbo.Contacts", "Id", cascadeDelete: true);
            DropColumn("dbo.SecurityItems", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SecurityItems", "Title", c => c.String());
            DropForeignKey("dbo.ContactLines", "Contact_Id", "dbo.Contacts");
            DropIndex("dbo.ContactLines", new[] { "Contact_Id" });
            DropPrimaryKey("dbo.SecurityItems");
            DropPrimaryKey("dbo.Contacts");
            AlterColumn("dbo.SecurityItems", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Contacts", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.ContactLines", "Contact_Id", c => c.Long());
            DropColumn("dbo.SecurityItems", "Name");
            AddPrimaryKey("dbo.SecurityItems", "Id");
            AddPrimaryKey("dbo.Contacts", "Id");
            CreateIndex("dbo.ContactLines", "Contact_Id");
            AddForeignKey("dbo.ContactLines", "Contact_Id", "dbo.Contacts", "Id", cascadeDelete: true);
        }
    }
}
