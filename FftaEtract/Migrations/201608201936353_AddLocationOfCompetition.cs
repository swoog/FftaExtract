namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class AddLocationOfCompetition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompetitionInfoes", "Location", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompetitionInfoes", "Location");
        }
    }
}
