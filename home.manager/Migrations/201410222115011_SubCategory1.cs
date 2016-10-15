namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubCategory1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubCategories", "Description", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubCategories", "Description", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
