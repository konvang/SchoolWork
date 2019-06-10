using System.Data.Entity;
using OrderEntryEngine;

namespace OrderEntryDataAccess
{
    public class OrderEntryContext : DbContext
    {
        public OrderEntryContext()
            : base("OrderEntryContext")
        {
            ////Database.Initialize(true);
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLine> Lines { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}