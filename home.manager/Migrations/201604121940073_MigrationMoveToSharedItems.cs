namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationMoveToSharedItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "Name", c => c.String());
            AlterColumn("dbo.Expenses", "Description", c => c.String());
            DropColumn("dbo.Expenses", "ItemAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Expenses", "ItemAmount", c => c.Double());
            AlterColumn("dbo.Expenses", "Description", c => c.String(maxLength: 50));
            DropColumn("dbo.Expenses", "Name");
        }
    }
}
