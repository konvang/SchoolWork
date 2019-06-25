using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class ReportViewModel : WorkspaceViewModel
    {
        public ReportViewModel()
            : base("Reports")
        {
            this.LoadReport();
        }

        public object CustomerOrders { get; set; }

        protected override void CreateCommands()
        {
        }

        private void LoadReport()
        {
            var report =
                from c in (RepositoryManager.GetRepository(typeof(Customer)) as Repository<Customer>).GetEntities()
                select new { Name = c.FirstName + " " + c.LastName, Orders = c.Orders, TotalToBeSpent = c.Orders.Sum(o => o.Total), TotalSpent = c.Orders.Where(o => o.Status == OrderEntryEngine.OrderStatus.Placed).Sum(o => o.Total) };

            this.CustomerOrders = report;
        }
    }
}