namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addsexearcher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Archers", "Sexe", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Archers", "Sexe");
        }
    }
}
