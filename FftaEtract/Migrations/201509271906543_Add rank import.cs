namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addrankimport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompetitionScores", "ImportedRank", c => c.Int());
            AlterColumn("dbo.CompetitionScores", "Rank", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompetitionScores", "Rank", c => c.Int(nullable: false));
            DropColumn("dbo.CompetitionScores", "ImportedRank");
        }
    }
}
