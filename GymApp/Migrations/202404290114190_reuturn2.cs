namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reuturn2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReturnItems", "ReturnId", c => c.Int(nullable: false));
            AddColumn("dbo.ReturnItems", "Order_Id", c => c.Int());
            CreateIndex("dbo.ReturnItems", "Order_Id");
            AddForeignKey("dbo.ReturnItems", "Order_Id", "dbo.Orders", "Id");
            DropColumn("dbo.ReturnItems", "ItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReturnItems", "ItemId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ReturnItems", "Order_Id", "dbo.Orders");
            DropIndex("dbo.ReturnItems", new[] { "Order_Id" });
            DropColumn("dbo.ReturnItems", "Order_Id");
            DropColumn("dbo.ReturnItems", "ReturnId");
        }
    }
}
