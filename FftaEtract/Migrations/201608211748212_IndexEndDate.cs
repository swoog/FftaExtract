namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IndexEndDate : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.JobInfoes", new[] { "JobStatus", "EndJob" }, name: "IX_EndJob");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobInfoes", "IX_EndJob");
        }
    }
}
