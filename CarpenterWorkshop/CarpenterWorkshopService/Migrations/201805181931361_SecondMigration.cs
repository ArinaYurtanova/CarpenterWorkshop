namespace CarpenterWorkshopService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MessageInfoes", "CustomerID");
            RenameColumn(table: "dbo.MessageInfoes", name: "Сustomer_Id", newName: "CustomerID");
            RenameIndex(table: "dbo.MessageInfoes", name: "IX_Сustomer_Id", newName: "IX_CustomerID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.MessageInfoes", name: "IX_CustomerID", newName: "IX_Сustomer_Id");
            RenameColumn(table: "dbo.MessageInfoes", name: "CustomerID", newName: "Сustomer_Id");
            AddColumn("dbo.MessageInfoes", "CustomerID", c => c.Int());
        }
    }
}
