namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addjobinfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        JobStatus = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JobInfoes");
        }
    }
}
