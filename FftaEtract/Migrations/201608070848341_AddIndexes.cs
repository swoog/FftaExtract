namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Clubs", "Name", unique: true, name: "IX_ClubName");
            CreateIndex("dbo.JobInfoes", new[] { "JobStatus", "CreatedDateTime" }, name: "IX_JobStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobInfoes", "IX_JobStatus");
            DropIndex("dbo.Clubs", "IX_ClubName");
        }
    }
}
