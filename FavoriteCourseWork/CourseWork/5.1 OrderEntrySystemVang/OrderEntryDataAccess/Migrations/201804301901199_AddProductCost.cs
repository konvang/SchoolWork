namespace OrderEntryDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductCost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Cost");
        }
    }
}
