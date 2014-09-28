namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archers",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        LastName = c.String(),
                        FirstName = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.CompetitionInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Competitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        CompetitionInfoId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompetitionInfoes", t => t.CompetitionInfoId, cascadeDelete: true)
                .Index(t => t.CompetitionInfoId);
            
            CreateTable(
                "dbo.CompetitionScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompetitionId = c.Int(nullable: false),
                        ArcherCode = c.String(maxLength: 128),
                        Score = c.Int(nullable: false),
                        BowType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Archers", t => t.ArcherCode)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId)
                .Index(t => t.ArcherCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompetitionScores", "CompetitionId", "dbo.Competitions");
            DropForeignKey("dbo.CompetitionScores", "ArcherCode", "dbo.Archers");
            DropForeignKey("dbo.Competitions", "CompetitionInfoId", "dbo.CompetitionInfoes");
            DropIndex("dbo.CompetitionScores", new[] { "ArcherCode" });
            DropIndex("dbo.CompetitionScores", new[] { "CompetitionId" });
            DropIndex("dbo.Competitions", new[] { "CompetitionInfoId" });
            DropTable("dbo.CompetitionScores");
            DropTable("dbo.Competitions");
            DropTable("dbo.CompetitionInfoes");
            DropTable("dbo.Archers");
        }
    }
}
