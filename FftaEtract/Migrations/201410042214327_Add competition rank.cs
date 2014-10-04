namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcompetitionrank : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompetitionScores", "Rank", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompetitionScores", "Rank");
        }
    }
}
