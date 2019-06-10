using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class OrderViewModel : EntityViewModel<Order>
    {
        private MultiOrderLineViewModel filteredLineViewModel;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="order">The car to be shown.</param>
        /// <param name="repository">The car repository.</param>
        public OrderViewModel(Order order )
            : base("New order", order)
        {
            this.Entity = order;
        
            this.filteredLineViewModel = new MultiOrderLineViewModel(this.Entity);
            this.filteredLineViewModel.AllLines = this.FilteredLines;

            this.OrderRepository = (Repository<Order>)RepositoryManager.GetRepository(typeof(Order));
        }

        public Repository<Order> OrderRepository { get; set; }


        public Order Order
        {
            get
            {
                return this.Entity;
            }
        }

        public decimal ProductTotal
        {
            get
            {
                return this.Entity.ProductTotal;
            }
        }

        public decimal TaxTotal
        {
            get
            {
                return this.Entity.TaxTotal;
            }
        }

        public decimal Total
        {
            get
            {
                return this.Entity.Total;
            }
        }

        public ObservableCollection<OrderLineViewModel> FilteredLines
        {
            get
            {
                List<OrderLineViewModel> lines = null;

                if (this.Entity.Lines != null)
                {
                    lines =
                        (from l in this.Entity.Lines
                         select new OrderLineViewModel(l)).ToList();
                }

                this.FilteredLineViewModel.AddPropertyChangedEvent(lines);

                return new ObservableCollection<OrderLineViewModel>(lines);
            }
        }

        public MultiOrderLineViewModel FilteredLineViewModel
        {
            get
            {
                return this.filteredLineViewModel;
            }
        }

        [EntityColumn(1, 75)]
        [EntityControl(ControlType.ComboBox, 1)]
        public OrderStatus Status
        {
            get
            {
                return this.Entity.Status;
            }
            set
            {
                this.Entity.Status = value;
            }
        }

        [EntityColumn(2, 75)]
        [EntityControl(ControlType.ComboBox, 2)]
        public Customer Customer
        {
            get
            {
                return this.Entity.Customer;
            }
            set
            {
                this.Entity.Customer = value;
            }
        }

        [EntityColumn(3, 75)]
        [EntityControl(ControlType.TextBox, 3)]
        public decimal ShippingAmount
        {
            get
            {
                return this.Entity.ShippingAmount;
            }
            set
            {
                this.Entity.ShippingAmount = value;
                this.OnPropertyChanged("ShippingAmount");
                this.OnPropertyChanged("Total");
            }
        }

        public IEnumerable<OrderStatus> OrderStatuses
        {
            get
            {
                return Enum.GetValues(typeof(OrderStatus)) as IEnumerable<OrderStatus>;
            }
        }

        public IEnumerable<Game> Products
        {
            get
            {
               return (RepositoryManager.GetRepository(typeof(Game)) as Repository<Game>).GetEntities();
            }
        }

        public void UpdateOrderTotals()
        {
            this.OnPropertyChanged("ProductTotal");
            this.OnPropertyChanged("TaxTotal");
            this.OnPropertyChanged("Total");
            this.OnPropertyChanged("Status");
        }
    }
}