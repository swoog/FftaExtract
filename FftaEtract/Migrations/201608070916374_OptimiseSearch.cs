namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptimiseSearch : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Archers", "LastName", name: "IX_Lastname");
            CreateIndex("dbo.Archers", "FirstName", name: "IX_Firstname");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Archers", "IX_Firstname");
            DropIndex("dbo.Archers", "IX_Lastname");
        }
    }
}
