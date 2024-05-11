namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveModels : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TrainerBookingsTrainers", newName: "TrainersTrainerBookings");
            DropForeignKey("dbo.Trainers", "ClassCreation_Id", "dbo.ClassCreations");
            DropIndex("dbo.Trainers", new[] { "ClassCreation_Id" });
            DropPrimaryKey("dbo.TrainersTrainerBookings");
            AddPrimaryKey("dbo.TrainersTrainerBookings", new[] { "Trainers_Id", "TrainerBookings_Id" });
            DropColumn("dbo.Trainers", "ClassCreation_Id");
            DropTable("dbo.ClassCreations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClassCreations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        TrainerId = c.Int(nullable: false),
                        Status = c.String(),
                        IsBooked = c.Int(nullable: false),
                        TrainerBookingId = c.Int(nullable: false),
                        AssignedTrainer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Trainers", "ClassCreation_Id", c => c.Int());
            DropPrimaryKey("dbo.TrainersTrainerBookings");
            AddPrimaryKey("dbo.TrainersTrainerBookings", new[] { "TrainerBookings_Id", "Trainers_Id" });
            CreateIndex("dbo.Trainers", "ClassCreation_Id");
            AddForeignKey("dbo.Trainers", "ClassCreation_Id", "dbo.ClassCreations", "Id");
            RenameTable(name: "dbo.TrainersTrainerBookings", newName: "TrainerBookingsTrainers");
        }
    }
}
