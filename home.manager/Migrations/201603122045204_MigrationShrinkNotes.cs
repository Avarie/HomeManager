namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationShrinkNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "PublicId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notes", "PublicId");
        }
    }
}
