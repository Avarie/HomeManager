namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationShringDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Owner_UserId", c => c.Int());
            CreateIndex("dbo.Documents", "Owner_UserId");
            AddForeignKey("dbo.Documents", "Owner_UserId", "dbo.UserProfile", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "Owner_UserId", "dbo.UserProfile");
            DropIndex("dbo.Documents", new[] { "Owner_UserId" });
            DropColumn("dbo.Documents", "Owner_UserId");
        }
    }
}
