namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddErrorMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobInfoes", "ReasonPhrase", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobInfoes", "ReasonPhrase");
        }
    }
}
