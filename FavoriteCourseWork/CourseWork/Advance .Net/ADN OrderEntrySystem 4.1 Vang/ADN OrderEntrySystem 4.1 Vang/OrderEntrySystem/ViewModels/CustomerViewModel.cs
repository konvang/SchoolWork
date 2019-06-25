using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class CustomerViewModel : EntityViewModel<Customer>, IDataErrorInfo
    {

        private MultiEntityViewModel<Order, OrderViewModel, EntityView> filteredOrderViewModel;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="customer">The car to be shown.</param>
        /// <param name="repository">The car repository.</param>
        public CustomerViewModel(Customer customer)
            : base("New customer", customer)
        {
            this.Entity = customer;

            this.filteredOrderViewModel = new MultiEntityViewModel<Order, OrderViewModel, EntityView>();
            this.filteredOrderViewModel.AllEntities = this.FilteredOrders;
            this.CustomerRepository = (Repository<Customer>)RepositoryManager.GetRepository(typeof(Customer));
        }

        public Repository<Customer> CustomerRepository { get; set; }

        public string Error
        {
            get
            {
                return this.Entity.Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.Entity[propertyName];
            }
        }

        [EntityColumn(1, 75)]
        [EntityControl(ControlType.TextBox, 1)]
        public string FirstName
        {
            get
            {
                return this.Entity.FirstName;
            }
            set
            {
                this.Entity.FirstName = value;
                this.OnPropertyChanged("FirstName");
            }
        }

        [EntityColumn(2, 75)]
        [EntityControl(ControlType.TextBox, 2)]
        public string LastName
        {
            get
            {
                return this.Entity.LastName;
            }
            set
            {
                this.Entity.LastName = value;
                this.OnPropertyChanged("LastName");
            }
        }

        [EntityColumn(3, 75)]
        [EntityControl(ControlType.TextBox, 3)]
        public string Phone
        {
            get
            {
                return this.Entity.Phone;
            }
            set
            {
                this.Entity.Phone = value;
                this.OnPropertyChanged("Phone");
            }
        }

        [EntityColumn(4, 75)]
        [EntityControl(ControlType.TextBox, 4)]
        public string Email
        {
            get
            {
                return this.Entity.Email;
            }
            set
            {
                this.Entity.Email = value;
                this.OnPropertyChanged("Email");
            }
        }

        public ObservableCollection<OrderViewModel> FilteredOrders
        {
            get
            {
                var orders =
                  (from o in this.Entity.Orders
                   select new OrderViewModel(o)).ToList();

                this.FilteredOrderViewModel.AddPropertyChangedEvent(orders);

                return new ObservableCollection<OrderViewModel>(orders);
            }
        }

        public MultiEntityViewModel<Order, OrderViewModel, EntityView> FilteredOrderViewModel
        {
            get
            {
                return this.filteredOrderViewModel;
            }
        }

        [EntityColumn(5, 75)]
        [EntityControl(ControlType.TextBox, 5)]
        public string Address
        {
            get
            {
                return this.Entity.Address;
            }
            set
            {
                this.Entity.Address = value;
                this.OnPropertyChanged("Address");
            }
        }

        [EntityColumn(6, 75)]
        [EntityControl(ControlType.TextBox, 6)]
        public string City
        {
            get
            {
                return this.Entity.City;
            }
            set
            {
                this.Entity.City = value;
                this.OnPropertyChanged("City");
            }
        }

        [EntityColumn(7, 75)]
        [EntityControl(ControlType.TextBox, 7)]
        public string State
        {
            get
            {
                return this.Entity.State;
            }
            set
            {
                this.Entity.State = value;
                this.OnPropertyChanged("State");
            }
        }

        public Customer Customer
        {
            get
            {
                return this.Entity;
            }
        }
    }
}