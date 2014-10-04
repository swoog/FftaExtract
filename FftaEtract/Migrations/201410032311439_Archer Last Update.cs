namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArcherLastUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Archers", "LastUpdate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Archers", "LastUpdate");
        }
    }
}
