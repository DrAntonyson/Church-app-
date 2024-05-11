namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bear : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Returns",
                c => new
                    {
                        ReturnId = c.Int(nullable: false, identity: true),
                        Id = c.Int(nullable: false),
                        ReturnReason = c.String(),
                        RequestedBy = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        imageUrl = c.String(),
                    })
                .PrimaryKey(t => t.ReturnId)
                .ForeignKey("dbo.Orders", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Returns", "Id", "dbo.Orders");
            DropIndex("dbo.Returns", new[] { "Id" });
            DropTable("dbo.Returns");
        }
    }
}
