namespace home.manager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTitleInit : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format("INSERT INTO Notes (Date, Content, Name, Description, Owner_UserId) VALUES ('{0}', N'<div class=\"hero-unit\">    <h1>Welcome!!!</h1>    <p>This project is designed to help with home management.</p>    <p>Current stage: Development.</p></div><h2>Road Map</h2><div class=\"row\">    <div class=\"span4\">        <h3>Improvement: .Net Account model</h3>        <p>It have to be reworked.</p>    </div>    <div class=\"span4\">        <h3>Feature: Charts</h3>        <p>The chartsData will give a possibility to analyze stored data in graphical view.</p>    </div>    <div class=\"span4\">        <h3>Feature: Documents</h3>        <p>With documents section we can store any image or other type file in our manager for quick and secure access in any cases by web.</p>    </div></div>', 'TITLE', 'TITLE', null );", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }
        
        public override void Down()
        {
        }
    }
}
