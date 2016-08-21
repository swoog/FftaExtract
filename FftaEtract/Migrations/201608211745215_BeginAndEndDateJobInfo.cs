namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeginAndEndDateJobInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobInfoes", "EndJob", c => c.DateTime());
            AddColumn("dbo.JobInfoes", "BeginJob", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobInfoes", "BeginJob");
            DropColumn("dbo.JobInfoes", "EndJob");
        }
    }
}
