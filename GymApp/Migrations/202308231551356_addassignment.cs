namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addassignment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassCreations", "AssignedTrainer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassCreations", "AssignedTrainer");
        }
    }
}
