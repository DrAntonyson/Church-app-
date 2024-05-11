namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassCreations", "Status", c => c.String());
            AddColumn("dbo.ClassCreations", "IsBooked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassCreations", "IsBooked");
            DropColumn("dbo.ClassCreations", "Status");
        }
    }
}
