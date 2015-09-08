namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcolumncode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Archers", "CodeArcher", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Archers", "CodeArcher");
        }
    }
}
