using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class OrderViewModel : WorkspaceViewModel
    {
        /// <summary>
        /// The car being shown.
        /// </summary>
        private Order order;

        /// <summary>
        /// The car view model's database repository.
        /// </summary>
        private Repository repository;

        /// <summary>
        /// An indicator of whether or not an car is selected.
        /// </summary>
        private bool isSelected;

        private MultiOrderLineViewModel filteredLineViewModel;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="order">The car to be shown.</param>
        /// <param name="repository">The car repository.</param>
        public OrderViewModel(Order order, Repository repository)
            : base("New order")
        {
            this.order = order;
            this.repository = repository;
            this.filteredLineViewModel = new MultiOrderLineViewModel(this.repository, this.order);
            this.filteredLineViewModel.AllLines = this.FilteredLines;
        }

        public Order Order
        {
            get
            {
                return this.order;
            }
        }

        public decimal ProductTotal
        {
            get
            {
                return this.order.ProductTotal;
            }
        }

        public decimal TaxTotal
        {
            get
            {
                return this.order.TaxTotal;
            }
        }

        public decimal Total
        {
            get
            {
                return this.order.Total;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this car is selected in the UI.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public ObservableCollection<OrderLineViewModel> FilteredLines
        {
            get
            {
                List<OrderLineViewModel> lines = null;

                if (this.order.Lines != null)
                {
                    lines =
                        (from l in this.order.Lines
                         select new OrderLineViewModel(l, this.repository)).ToList();
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

        public OrderStatus Status
        {
            get
            {
                return this.order.Status;
            }
            set
            {
                this.order.Status = value;
            }
        }

        public Customer Customer
        {
            get
            {
                return this.order.Customer;
            }
            set
            {
                this.order.Customer = value;
            }
        }

        public decimal ShippingAmount
        {
            get
            {
                return this.order.ShippingAmount;
            }
            set
            {
                this.order.ShippingAmount = value;
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

        public IEnumerable<Product> Products
        {
            get
            {
                return this.repository.GetProducts();
            }
        }

        public void UpdateOrderTotals()
        {
            this.OnPropertyChanged("ProductTotal");
            this.OnPropertyChanged("TaxTotal");
            this.OnPropertyChanged("Total");
            this.OnPropertyChanged("Status");
        }

        /// <summary>
        /// Creates the commands needed for the car view model.
        /// </summary>
        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("OK", new DelegateCommand(p => this.OkExecute()), true, false));
            this.Commands.Add(new CommandViewModel("Cancel", new DelegateCommand(p => this.CancelExecute()), false, true));
        }

        /// <summary>
        /// Saves the car view model's car to the repository.
        /// </summary>
        private void Save()
        {
            // Add car to repository.
            this.repository.AddOrder(this.order);

            this.repository.SaveToDatabase();
        }

        /// <summary>
        /// Saves the car and closes the new car window.
        /// </summary>
        private void OkExecute()
        {
            this.Save();
            this.CloseAction(true);
        }

        /// <summary>
        /// Closes the new car window without saving.
        /// </summary>
        private void CancelExecute()
        {
            this.CloseAction(false);
        }
    }
}