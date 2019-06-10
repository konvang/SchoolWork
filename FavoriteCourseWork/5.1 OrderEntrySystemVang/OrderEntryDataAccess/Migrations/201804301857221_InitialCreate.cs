namespace OrderEntryDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LocationId = c.Int(nullable: false),
                        Condition = c.Int(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        City = c.String(nullable: false, maxLength: 100),
                        State = c.String(nullable: false, maxLength: 2),
                        IsArchived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                        ProductAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxPerProduct = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                        ShippingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(maxLength: 15),
                        Email = c.String(nullable: false, maxLength: 255),
                        Address = c.String(maxLength: 150),
                        City = c.String(maxLength: 100),
                        State = c.String(maxLength: 2),
                        IsArchived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategories", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderLines", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderLines", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Products", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.ProductCategories", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.OrderLines", new[] { "OrderId" });
            DropIndex("dbo.OrderLines", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "LocationId" });
            DropIndex("dbo.ProductCategories", new[] { "CategoryId" });
            DropIndex("dbo.ProductCategories", new[] { "ProductId" });
            DropTable("dbo.Customers");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderLines");
            DropTable("dbo.Locations");
            DropTable("dbo.Products");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Categories");
        }
    }
}
