namespace FftaExtract.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptimiseSearch : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Archers", "LastName", c => c.String(maxLength: 200));
            AlterColumn("dbo.Archers", "FirstName", c => c.String(maxLength: 200));
            CreateIndex("dbo.Archers", "LastName", name: "IX_Lastname");
            CreateIndex("dbo.Archers", "FirstName", name: "IX_Firstname");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Archers", "IX_Firstname");
            DropIndex("dbo.Archers", "IX_Lastname");
            AlterColumn("dbo.Archers", "FirstName", c => c.String());
            AlterColumn("dbo.Archers", "LastName", c => c.String());
        }
    }
}
