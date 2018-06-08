namespace CarpenterWorkshopService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlankCrafts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WoodBlanksID = c.Int(nullable: false),
                        WoodCraftsID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WoodBlanks", t => t.WoodBlanksID, cascadeDelete: true)
                .ForeignKey("dbo.WoodCrafts", t => t.WoodCraftsID, cascadeDelete: true)
                .Index(t => t.WoodBlanksID)
                .Index(t => t.WoodCraftsID);
            
            CreateTable(
                "dbo.WoodBlanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WoodBlanksName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StorageBlanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageID = c.Int(nullable: false),
                        WoodBlanksID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Storages", t => t.StorageID, cascadeDelete: true)
                .ForeignKey("dbo.WoodBlanks", t => t.WoodBlanksID, cascadeDelete: true)
                .Index(t => t.StorageID)
                .Index(t => t.WoodBlanksID);
            
            CreateTable(
                "dbo.Storages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WoodCrafts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WoodCraftsName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrdProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        WoodCraftsID = c.Int(nullable: false),
                        WorkerID = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Сustomer", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.WoodCrafts", t => t.WoodCraftsID, cascadeDelete: true)
                .ForeignKey("dbo.Workers", t => t.WorkerID)
                .Index(t => t.CustomerID)
                .Index(t => t.WoodCraftsID)
                .Index(t => t.WorkerID);
            
            CreateTable(
                "dbo.Сustomer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerFIO = c.String(nullable: false),
                        Mail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.String(),
                        FromMailAddress = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        DateDelivery = c.DateTime(nullable: false),
                        CustomerID = c.Int(),
                        Сustomer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Сustomer", t => t.Сustomer_Id)
                .Index(t => t.Сustomer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageInfoes", "Сustomer_Id", "dbo.Сustomer");
            DropForeignKey("dbo.OrdProducts", "WorkerID", "dbo.Workers");
            DropForeignKey("dbo.OrdProducts", "WoodCraftsID", "dbo.WoodCrafts");
            DropForeignKey("dbo.OrdProducts", "CustomerID", "dbo.Сustomer");
            DropForeignKey("dbo.BlankCrafts", "WoodCraftsID", "dbo.WoodCrafts");
            DropForeignKey("dbo.StorageBlanks", "WoodBlanksID", "dbo.WoodBlanks");
            DropForeignKey("dbo.StorageBlanks", "StorageID", "dbo.Storages");
            DropForeignKey("dbo.BlankCrafts", "WoodBlanksID", "dbo.WoodBlanks");
            DropIndex("dbo.MessageInfoes", new[] { "Сustomer_Id" });
            DropIndex("dbo.OrdProducts", new[] { "WorkerID" });
            DropIndex("dbo.OrdProducts", new[] { "WoodCraftsID" });
            DropIndex("dbo.OrdProducts", new[] { "CustomerID" });
            DropIndex("dbo.StorageBlanks", new[] { "WoodBlanksID" });
            DropIndex("dbo.StorageBlanks", new[] { "StorageID" });
            DropIndex("dbo.BlankCrafts", new[] { "WoodCraftsID" });
            DropIndex("dbo.BlankCrafts", new[] { "WoodBlanksID" });
            DropTable("dbo.MessageInfoes");
            DropTable("dbo.Workers");
            DropTable("dbo.Сustomer");
            DropTable("dbo.OrdProducts");
            DropTable("dbo.WoodCrafts");
            DropTable("dbo.Storages");
            DropTable("dbo.StorageBlanks");
            DropTable("dbo.WoodBlanks");
            DropTable("dbo.BlankCrafts");
        }
    }
}
