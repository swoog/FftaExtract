namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexOnUrl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobInfoes", "Url", c => c.String(maxLength: 300));
            CreateIndex("dbo.JobInfoes", new[] { "JobStatus", "Url" }, name: "IX_Url");
        }
        
        public override void Down()
        {
            DropIndex("dbo.JobInfoes", "IX_Url");
            AlterColumn("dbo.JobInfoes", "Url", c => c.String());
        }
    }
}
