using OrderEntryEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntryDataAccess
{
    public static class RepositoryManager
    {

        public static OrderEntryContext Context { get; set; }

        public static Dictionary<Type, IRepository> Repositories = new Dictionary<Type, IRepository>();

        public static void IntitializeRepository(OrderEntryContext context)
        {
            RepositoryManager.Context = new OrderEntryContext();
            Repositories = new Dictionary<Type, IRepository>();

            Repositories.Add(typeof(Category), new LookupRepository<Category>(context.Categories));
            Repositories.Add(typeof(Game), new LookupRepository<Game>(context.Games));
            Repositories.Add(typeof(Customer), new LookupRepository<Customer>(context.Customers));
            Repositories.Add(typeof(Location), new LookupRepository<Location>(context.Locations));

            Repositories.Add(typeof(ProductCategory), new Repository<ProductCategory>(context.ProductCategories));
            Repositories.Add(typeof(OrderLine), new Repository<OrderLine>(context.Lines));
            Repositories.Add(typeof(Order), new Repository<Order>(context.Orders));
        }

        public static IRepository GetRepository(Type type)
        {
            IRepository value = null;

            if (Repositories.TryGetValue(type, out value))
            {
                return value;
            }
            else
            {
                throw new Exception("There is no repoistory for the type.");
            }
        }

        public static ILookupRepository GetLookupRepository(Type type)
        {
            IRepository value;
            Repositories.TryGetValue(type, out value);
            return (value as ILookupRepository);
        }
    }
}
