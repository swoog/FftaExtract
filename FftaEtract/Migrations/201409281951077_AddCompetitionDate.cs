namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompetitionDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitions", "End", c => c.DateTime(nullable: false));
            AddColumn("dbo.Competitions", "Begin", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Competitions", "Begin");
            DropColumn("dbo.Competitions", "End");
        }
    }
}
