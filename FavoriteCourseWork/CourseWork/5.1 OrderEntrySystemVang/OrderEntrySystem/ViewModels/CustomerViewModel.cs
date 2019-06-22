using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class CustomerViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        /// <summary>
        /// The car being shown.
        /// </summary>
        private Customer customer;

        /// <summary>
        /// The car view model's database repository.
        /// </summary>
        private Repository repository;

        /// <summary>
        /// An indicator of whether or not an car is selected.
        /// </summary>
        private bool isSelected;

        private MultiOrderViewModel filteredOrderViewModel;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="customer">The car to be shown.</param>
        /// <param name="repository">The car repository.</param>
        public CustomerViewModel(Customer customer, Repository repository)
            : base("New customer")
        {
            this.customer = customer;
            this.repository = repository;
            this.filteredOrderViewModel = new MultiOrderViewModel(this.repository, this.customer);
            this.filteredOrderViewModel.AllOrders = this.FilteredOrders;
        }

        public string Error
        {
            get
            {
                return this.customer.Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.customer[propertyName];
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

        public string FirstName
        {
            get
            {
                return this.customer.FirstName;
            }
            set
            {
                this.customer.FirstName = value;
                this.OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get
            {
                return this.customer.LastName;
            }
            set
            {
                this.customer.LastName = value;
                this.OnPropertyChanged("LastName");
            }
        }

        public string Phone
        {
            get
            {
                return this.customer.Phone;
            }
            set
            {
                this.customer.Phone = value;
                this.OnPropertyChanged("Phone");
            }
        }

        public string Email
        {
            get
            {
                return this.customer.Email;
            }
            set
            {
                this.customer.Email = value;
                this.OnPropertyChanged("Email");
            }
        }

        public ObservableCollection<OrderViewModel> FilteredOrders
        {
            get
            {
                var orders =
                    (from o in this.customer.Orders
                    select new OrderViewModel(o, this.repository)).ToList();

                this.FilteredOrderViewModel.AddPropertyChangedEvent(orders);

                return new ObservableCollection<OrderViewModel>(orders);
            }
        }

        public MultiOrderViewModel FilteredOrderViewModel
        {
            get
            {
                return this.filteredOrderViewModel;
            }
        }

        public string Address
        {
            get
            {
                return this.customer.Address;
            }
            set
            {
                this.customer.Address = value;
                this.OnPropertyChanged("Address");
            }
        }

        public string City
        {
            get
            {
                return this.customer.City;
            }
            set
            {
                this.customer.City = value;
                this.OnPropertyChanged("City");
            }
        }

        public string State
        {
            get
            {
                return this.customer.State;
            }
            set
            {
                this.customer.State = value;
                this.OnPropertyChanged("State");
            }
        }

        public Customer Customer
        {
            get
            {
                return this.customer;
            }
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
        private bool Save()
        {
            bool result = true;

            if (this.Customer.IsValid)
            {
                // Add customer to repository.
                this.repository.AddCustomer(this.customer);

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("One or more properties are invalid. Customer could not be saved.");
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Saves the car and closes the new car window.
        /// </summary>
        private void OkExecute()
        {
            if (this.Save())
            {
                this.CloseAction(true);
            }
        }

        /// <summary>
        /// Closes the new customer window without saving.
        /// </summary>
        private void CancelExecute()
        {
            this.CloseAction(false);
        }
    }
}