namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClassCreations", "IsBooked", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClassCreations", "IsBooked", c => c.Boolean(nullable: false));
        }
    }
}
