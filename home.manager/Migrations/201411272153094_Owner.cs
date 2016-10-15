namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Owner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Owner_UserId", c => c.Int());
            CreateIndex("dbo.Expenses", "Owner_UserId");
            AddForeignKey("dbo.Expenses", "Owner_UserId", "dbo.UserProfile", "UserId");
            DropColumn("dbo.Expenses", "Owner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "Owner", c => c.Int(nullable: false));
            DropForeignKey("dbo.Expenses", "Owner_UserId", "dbo.UserProfile");
            DropIndex("dbo.Expenses", new[] { "Owner_UserId" });
            DropColumn("dbo.Expenses", "Owner_UserId");
        }
    }
}
