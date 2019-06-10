namespace OrderEntryDataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerUsername : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Username", c => c.String());
            Sql("UPDATE dbo.Customers SET Username = LOWER(FirstName + LastName) WHERE Username IS NULL");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Username");
        }
    }
}
