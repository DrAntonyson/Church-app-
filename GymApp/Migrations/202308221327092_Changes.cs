namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TrainersClassCreations", "Trainers_Id", "dbo.Trainers");
            DropForeignKey("dbo.TrainersClassCreations", "ClassCreation_Id", "dbo.ClassCreations");
            DropIndex("dbo.TrainersClassCreations", new[] { "Trainers_Id" });
            DropIndex("dbo.TrainersClassCreations", new[] { "ClassCreation_Id" });
            AddColumn("dbo.Trainers", "ClassCreation_Id", c => c.Int());
            CreateIndex("dbo.Trainers", "ClassCreation_Id");
            AddForeignKey("dbo.Trainers", "ClassCreation_Id", "dbo.ClassCreations", "Id");
            DropColumn("dbo.Trainers", "ClassCreationId");
            DropTable("dbo.TrainersClassCreations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TrainersClassCreations",
                c => new
                    {
                        Trainers_Id = c.Int(nullable: false),
                        ClassCreation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trainers_Id, t.ClassCreation_Id });
            
            AddColumn("dbo.Trainers", "ClassCreationId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Trainers", "ClassCreation_Id", "dbo.ClassCreations");
            DropIndex("dbo.Trainers", new[] { "ClassCreation_Id" });
            DropColumn("dbo.Trainers", "ClassCreation_Id");
            CreateIndex("dbo.TrainersClassCreations", "ClassCreation_Id");
            CreateIndex("dbo.TrainersClassCreations", "Trainers_Id");
            AddForeignKey("dbo.TrainersClassCreations", "ClassCreation_Id", "dbo.ClassCreations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TrainersClassCreations", "Trainers_Id", "dbo.Trainers", "Id", cascadeDelete: true);
        }
    }
}
