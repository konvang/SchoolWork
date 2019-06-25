using System;
using System.Collections.Generic;
using System.Linq;
using OrderEntryEngine;

namespace OrderEntryDataAccess
{
    public class Repository
    {
        private OrderEntryContext db = new OrderEntryContext();

        public event EventHandler<ProductEventArgs> ProductAdded;

        public event EventHandler<CustomerEventArgs> CustomerAdded;

        public event EventHandler<LocationEventArgs> LocationAdded;

        public event EventHandler<CategoryEventArgs> CategoryAdded;

        public event EventHandler<OrderEventArgs> OrderAdded;

        public event EventHandler<OrderLineEventArgs> OrderLineAdded;

        public event EventHandler<ProductEventArgs> ProductRemoved;

        public event EventHandler<LocationEventArgs> LocationRemoved;

        public event EventHandler<OrderEventArgs> OrderRemoved;

        public event EventHandler<CustomerEventArgs> CustomerRemoved;

        public event EventHandler<CategoryEventArgs> CategoryRemoved;

        public event EventHandler<OrderLineEventArgs> OrderLineRemoved;

        public void AddProduct(Product product)
        {
            if (!this.ContainsProduct(product))
            {
                this.db.Products.Add(product);

                if (this.ProductAdded != null)
                {
                    this.ProductAdded(this, new ProductEventArgs(product));
                }
            }
        }

        public bool ContainsProduct(Product product)
        {
            return this.GetProduct(product.Id) != null;
        }

        public Product GetProduct(int id)
        {
            return this.db.Products.Find(id);
        }

        public List<Product> GetProducts()
        {
            return this.db.Products.Where(p => !p.IsArchived).ToList();
        }

        public void AddCustomer(Customer customer)
        {
            if (!this.ContainsCustomer(customer))
            {
                this.db.Customers.Add(customer);

                if (this.CustomerAdded != null)
                {
                    this.CustomerAdded(this, new CustomerEventArgs(customer));
                }
            }
        }

        public bool ContainsCustomer(Customer customer)
        {
            return this.GetCustomer(customer.Id) != null;
        }

        public Customer GetCustomer(int id)
        {
            return this.db.Customers.Find(id);
        }

        public List<Customer> GetCustomers()
        {
            return this.db.Customers.Where(c => !c.IsArchived).ToList();
        }

        public void AddLocation(Location location)
        {
            if (!this.ContainsLocation(location))
            {
                this.db.Locations.Add(location);

                if (this.LocationAdded != null)
                {
                    this.LocationAdded(this, new LocationEventArgs(location));
                }
            }
        }

        public bool ContainsLocation(Location location)
        {
            return this.GetLocation(location.Id) != null;
        }

        public Location GetLocation(int id)
        {
            return this.db.Locations.Find(id);
        }

        public List<Location> GetLocations()
        {
            return this.db.Locations.Where(l => !l.IsArchived).ToList();
        }

        public void AddCategory(Category category)
        {
            if (!this.ContainsCategory(category))
            {
                this.db.Categories.Add(category);

                if (this.CategoryAdded != null)
                {
                    this.CategoryAdded(this, new CategoryEventArgs(category));
                }
            }
        }

        public bool ContainsCategory(Category category)
        {
            return this.GetCategory(category.Id) != null;
        }

        public Category GetCategory(int id)
        {
            return this.db.Categories.Find(id);
        }

        public List<Category> GetCategories()
        {
            return this.db.Categories.Where(c => !c.IsArchived).ToList();
        }

        public void AddOrder(Order order)
        {
            if (!this.ContainsOrder(order))
            {
                this.db.Orders.Add(order);

                if (this.OrderAdded != null)
                {
                    this.OrderAdded(this, new OrderEventArgs(order));
                }
            }
        }

        public bool ContainsOrder(Order order)
        {
            return this.GetOrder(order.Id) != null;
        }

        public Order GetOrder(int id)
        {
            return this.db.Orders.Find(id);
        }

        public List<Order> GetOrders()
        {
            return this.db.Orders.Where(o => !o.IsArchived).ToList();
        }

        public void AddLine(OrderLine line)
        {
            if (!this.ContainsLine(line))
            {
                this.db.Lines.Add(line);

                if (this.OrderLineAdded != null)
                {
                    this.OrderLineAdded(this, new OrderLineEventArgs(line));
                }
            }
        }

        public bool ContainsLine(OrderLine line)
        {
            return this.GetLine(line.Id) != null;
        }

        public OrderLine GetLine(int id)
        {
            return this.db.Lines.Find(id);
        }

        public List<OrderLine> GetLines()
        {
            return this.db.Lines.Where(l => !l.IsArchived).ToList();
        }

        public void AddProductCategory(ProductCategory pc)
        {
            if (!this.ContainsProductCategory(pc))
            {
                this.db.ProductCategories.Add(pc);

                if (this.CategoryAdded != null)
                {
                    this.CategoryAdded(this, new CategoryEventArgs(pc.Category));
                }
            }
        }

        public bool ContainsProductCategory(ProductCategory pc)
        {
            return this.GetProductCategory(pc.Id) != null;
        }

        public ProductCategory GetProductCategory(int id)
        {
            return this.db.ProductCategories.Find(id);
        }

        public List<ProductCategory> GetProductCategories()
        {
            return this.db.ProductCategories.Where(pc => !pc.IsArchived).ToList();
        }

        public void RemoveCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            customer.IsArchived = true;

            if (this.CustomerRemoved != null)
            {
                this.CustomerRemoved(this, new CustomerEventArgs(customer));
            }
        }

        public void RemoveProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            product.IsArchived = true;

            if (this.ProductRemoved != null)
            {
                this.ProductRemoved(this, new ProductEventArgs(product));
            }
        }

        public void RemoveLine(OrderLine line)
        {
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }

            line.IsArchived = true;

            if (this.OrderLineRemoved != null)
            {
                this.OrderLineRemoved(this, new OrderLineEventArgs(line));
            }
        }

        public void RemoveLocation(Location location)
        {
            if (location == null)
            {
                throw new ArgumentNullException("location");
            }

            location.IsArchived = true;

            if (this.LocationRemoved != null)
            {
                this.LocationRemoved(this, new LocationEventArgs(location));
            }
        }

        public void RemoveOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            order.IsArchived = true;

            if (this.OrderRemoved != null)
            {
                this.OrderRemoved(this, new OrderEventArgs(order));
            }
        }

        public void RemoveCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }

            category.IsArchived = true;

            if (this.CategoryRemoved != null)
            {
                this.CategoryRemoved(this, new CategoryEventArgs(category));
            }
        }

        public void RemoveProductCategory(ProductCategory pc)
        {
            if (pc == null)
            {
                throw new ArgumentNullException("pc");
            }

            pc.IsArchived = true;

            if (this.CategoryRemoved != null)
            {
                this.CategoryRemoved(this, new CategoryEventArgs(pc.Category));
            }
        }

        public void SaveToDatabase()
        {
            this.db.SaveChanges();
        }
    }
}