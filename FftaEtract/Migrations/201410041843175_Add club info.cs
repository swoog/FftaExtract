namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addclubinfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArcherClubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArcherCode = c.String(maxLength: 128),
                        Year = c.Int(nullable: false),
                        ClubId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Archers", t => t.ArcherCode)
                .ForeignKey("dbo.Clubs", t => t.ClubId, cascadeDelete: true)
                .Index(t => t.ArcherCode)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArcherClubs", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.ArcherClubs", "ArcherCode", "dbo.Archers");
            DropIndex("dbo.ArcherClubs", new[] { "ClubId" });
            DropIndex("dbo.ArcherClubs", new[] { "ArcherCode" });
            DropTable("dbo.Clubs");
            DropTable("dbo.ArcherClubs");
        }
    }
}
