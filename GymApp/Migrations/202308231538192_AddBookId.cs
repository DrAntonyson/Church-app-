namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassCreations", "TrainerBookingId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassCreations", "TrainerBookingId");
        }
    }
}
