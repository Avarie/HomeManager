namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removerUsers : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.rUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.rUsers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.Id, t.Name });
            
        }
    }
}
