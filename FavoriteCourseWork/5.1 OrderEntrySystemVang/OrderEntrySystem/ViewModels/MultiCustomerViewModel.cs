using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class MultiCustomerViewModel : WorkspaceViewModel
    {
        private Repository repository;

        public MultiCustomerViewModel(Repository repository)
            : base("All customers")
        {
            this.repository = repository;

            List<CustomerViewModel> customers =
                (from customer in this.repository.GetCustomers()
                select new CustomerViewModel(customer, this.repository)).ToList();

            customers.ForEach(cvm => cvm.PropertyChanged += this.OnCustomerViewModelPropertyChanged);

            this.AllCustomers = new ObservableCollection<CustomerViewModel>(customers);

            this.repository.CustomerAdded += this.OnCustomerAdded;
            this.repository.CustomerRemoved += this.OnCustomerRemoved;
        }

        public ObservableCollection<CustomerViewModel> AllCustomers { get; private set; }

        public int NumberOfItemsSelected
        {
            get
            {
                return this.AllCustomers.Count(vm => vm.IsSelected);
            }
        }

        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("New...", new DelegateCommand(param => this.CreateNewCustomerExecute())));
            this.Commands.Add(new CommandViewModel("Edit...", new DelegateCommand(param => this.EditCustomerExecute(), p => this.NumberOfItemsSelected == 1)));
            this.Commands.Add(new CommandViewModel("Delete", new DelegateCommand(param => this.DeleteCustomerExecute(), p => this.NumberOfItemsSelected == 1)));
        }

        private void OnCustomerAdded(object sender, CustomerEventArgs e)
        {
            CustomerViewModel vm = new CustomerViewModel(e.Customer, this.repository);
            vm.PropertyChanged += this.OnCustomerViewModelPropertyChanged;

            this.AllCustomers.Add(vm);
        }

        /// <summary>
        /// A handler which responds when a customer view model's property changes.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCustomerViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string isSelected = "IsSelected";

            if (e.PropertyName == isSelected)
            {
                this.OnPropertyChanged("NumberOfItemsSelected");
            }
        }

        private void CreateNewCustomerExecute()
        {
            Customer customer = new Customer();

            CustomerViewModel viewModel = new CustomerViewModel(customer, this.repository);

            this.ShowCustomer(viewModel);
        }

        private void EditCustomerExecute()
        {
            CustomerViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                this.ShowCustomer(viewModel);

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("Please select only one car");
            }
        }

        private void DeleteCustomerExecute()
        {
            CustomerViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                if (MessageBox.Show("Do you really want to delete the selected customer?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.repository.RemoveCustomer(viewModel.Customer);
                    this.repository.SaveToDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select only one customer");
            }
        }

        private CustomerViewModel GetOnlySelectedViewModel()
        {
            CustomerViewModel result;

            try
            {
                result = this.AllCustomers.Single(vm => vm.IsSelected);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Creates a new window to edit a customer.
        /// </summary>
        /// <param name="viewModel">The view model for the customer to be edited.</param>
        private void ShowCustomer(CustomerViewModel viewModel)
        {
            WorkspaceWindow window = new WorkspaceWindow();
            window.Width = 400;
            viewModel.CloseAction = b => window.DialogResult = b;
            window.Title = viewModel.DisplayName;

            CustomerView view = new CustomerView();

            view.DataContext = viewModel;

            window.Content = view;

            window.ShowDialog();
        }

        private void OnCustomerRemoved(object sender, CustomerEventArgs e)
        {
            CustomerViewModel viewModel = this.GetOnlySelectedViewModel();
            if (viewModel != null)
            {
                if (viewModel.Customer == e.Customer)
                {
                    this.AllCustomers.Remove(viewModel);
                }
            }
        }
    }
}