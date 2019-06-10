using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderEntryDataAccess;

namespace OrderEntrySystem
{
    public class ReportViewModel : WorkspaceViewModel
    {
        private Repository repository;

        public ReportViewModel(Repository repository)
            : base("Reports")
        {
            this.repository = repository;
            this.LoadReport();
        }

        public object CustomerOrders { get; set; }

        protected override void CreateCommands()
        {
        }

        private void LoadReport()
        {
            var report =
                from c in this.repository.GetCustomers()
                select new { Name = c.FirstName + " " + c.LastName, Orders = c.Orders, TotalToBeSpent = c.Orders.Sum(o => o.Total), TotalSpent = c.Orders.Where(o => o.Status == OrderEntryEngine.OrderStatus.Placed).Sum(o => o.Total) };

            this.CustomerOrders = report;
        }
    }
}