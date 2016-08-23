namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndexNameOnCompetitionInfo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompetitionInfoes", "Name", c => c.String(maxLength: 200));
            CreateIndex("dbo.CompetitionInfoes", "Name", unique: true, name: "IX_CompetitionName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CompetitionInfoes", "IX_CompetitionName");
            AlterColumn("dbo.CompetitionInfoes", "Name", c => c.String());
        }
    }
}
