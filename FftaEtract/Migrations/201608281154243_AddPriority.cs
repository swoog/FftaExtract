namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPriority : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JobInfoes", "IX_JobStatus");
            AddColumn("dbo.JobInfoes", "Priority", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.JobInfoes", new[] { "JobStatus", "Priority", "CreatedDateTime" }, name: "IX_JobStatus");
        }

        public override void Down()
        {
            DropIndex("dbo.JobInfoes", "IX_JobStatus");
            DropColumn("dbo.JobInfoes", "Priority");
            CreateIndex("dbo.JobInfoes", new[] { "JobStatus", "CreatedDateTime" }, name: "IX_JobStatus");
        }
    }
}
