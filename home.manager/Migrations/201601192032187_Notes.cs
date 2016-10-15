namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NoteCategories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Content = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        Owner_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.Owner_UserId)
                .Index(t => t.Owner_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "Owner_UserId", "dbo.UserProfile");
            DropIndex("dbo.Notes", new[] { "Owner_UserId" });
            DropTable("dbo.Notes");
            DropTable("dbo.NoteCategories");
        }
    }
}
