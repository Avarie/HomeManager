namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationMoveToSharedCategories : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "Name", c => c.String());
            AlterColumn("dbo.Categories", "Description", c => c.String());
            AlterColumn("dbo.SubCategories", "Name", c => c.String());
            AlterColumn("dbo.SubCategories", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubCategories", "Description", c => c.String(maxLength: 50));
            AlterColumn("dbo.SubCategories", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Categories", "Description", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
