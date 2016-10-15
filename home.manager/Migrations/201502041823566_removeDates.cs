namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeDates : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Date");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Date",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
