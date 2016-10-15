namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationContactEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactCategories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactLines",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        Contact_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.Contact_Id, cascadeDelete: true)
                .Index(t => t.Contact_Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PublicId = c.String(),
                        Date = c.DateTime(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Category_Id = c.Long(),
                        Owner_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactCategories", t => t.Category_Id)
                .ForeignKey("dbo.UserProfile", t => t.Owner_UserId)
                .Index(t => t.Category_Id)
                .Index(t => t.Owner_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "Owner_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ContactLines", "Contact_Id", "dbo.Contacts");
            DropForeignKey("dbo.Contacts", "Category_Id", "dbo.ContactCategories");
            DropIndex("dbo.Contacts", new[] { "Owner_UserId" });
            DropIndex("dbo.Contacts", new[] { "Category_Id" });
            DropIndex("dbo.ContactLines", new[] { "Contact_Id" });
            DropTable("dbo.Contacts");
            DropTable("dbo.ContactLines");
            DropTable("dbo.ContactCategories");
        }
    }
}
