namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChanges : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TrainersTrainerBookings", newName: "TrainerBookingsTrainers");
            DropPrimaryKey("dbo.TrainerBookingsTrainers");
            CreateTable(
                "dbo.ClassCreations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        TrainerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainersClassCreations",
                c => new
                    {
                        Trainers_Id = c.Int(nullable: false),
                        ClassCreation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trainers_Id, t.ClassCreation_Id })
                .ForeignKey("dbo.Trainers", t => t.Trainers_Id, cascadeDelete: true)
                .ForeignKey("dbo.ClassCreations", t => t.ClassCreation_Id, cascadeDelete: true)
                .Index(t => t.Trainers_Id)
                .Index(t => t.ClassCreation_Id);
            
            AddColumn("dbo.Trainers", "ClassCreationId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TrainerBookingsTrainers", new[] { "TrainerBookings_Id", "Trainers_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainersClassCreations", "ClassCreation_Id", "dbo.ClassCreations");
            DropForeignKey("dbo.TrainersClassCreations", "Trainers_Id", "dbo.Trainers");
            DropIndex("dbo.TrainersClassCreations", new[] { "ClassCreation_Id" });
            DropIndex("dbo.TrainersClassCreations", new[] { "Trainers_Id" });
            DropPrimaryKey("dbo.TrainerBookingsTrainers");
            DropColumn("dbo.Trainers", "ClassCreationId");
            DropTable("dbo.TrainersClassCreations");
            DropTable("dbo.ClassCreations");
            AddPrimaryKey("dbo.TrainerBookingsTrainers", new[] { "Trainers_Id", "TrainerBookings_Id" });
            RenameTable(name: "dbo.TrainerBookingsTrainers", newName: "TrainersTrainerBookings");
        }
    }
}
