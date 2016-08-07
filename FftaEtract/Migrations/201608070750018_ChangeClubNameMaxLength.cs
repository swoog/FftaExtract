namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeClubNameMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clubs", "Name", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clubs", "Name", c => c.String());
        }
    }
}
