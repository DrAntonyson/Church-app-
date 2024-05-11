namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookClasses",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        classtypeid = c.Int(nullable: false),
                        datepicker = c.String(),
                        TrainerId = c.Int(),
                        UserBooked = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.classtypes", t => t.classtypeid, cascadeDelete: true)
                .ForeignKey("dbo.Trainers1", t => t.TrainerId)
                .Index(t => t.classtypeid)
                .Index(t => t.TrainerId);
            
            CreateTable(
                "dbo.classtypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        typeclass = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Trainers1",
                c => new
                    {
                        TrainerId = c.Int(nullable: false, identity: true),
                        TrainerRegistrationId = c.String(),
                        Name = c.String(nullable: false),
                        UserId = c.String(),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        IsAssigned = c.Boolean(nullable: false),
                        IsAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TrainerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookClasses", "TrainerId", "dbo.Trainers1");
            DropForeignKey("dbo.BookClasses", "classtypeid", "dbo.classtypes");
            DropIndex("dbo.BookClasses", new[] { "TrainerId" });
            DropIndex("dbo.BookClasses", new[] { "classtypeid" });
            DropTable("dbo.Trainers1");
            DropTable("dbo.classtypes");
            DropTable("dbo.BookClasses");
        }
    }
}
