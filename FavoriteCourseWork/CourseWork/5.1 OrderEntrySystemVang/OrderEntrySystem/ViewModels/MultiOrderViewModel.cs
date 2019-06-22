using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class MultiOrderViewModel : WorkspaceViewModel
    {
        private Repository repository;

        private Customer customer;

        public MultiOrderViewModel(Repository repository, Customer customer)
            : base("All orders")
        {
            this.repository = repository;
            this.customer = customer;

            this.Commands.Clear();
            this.CreateCommands();

            List<OrderViewModel> orders =
                (from order in this.repository.GetOrders()
                 select new OrderViewModel(order, this.repository)).ToList();

            this.AddPropertyChangedEvent(orders);

            this.AllOrders = new ObservableCollection<OrderViewModel>(orders);

            this.repository.OrderAdded += this.OnOrderAdded;
            this.repository.OrderRemoved += this.OnOrderRemoved;
        }

        public ObservableCollection<OrderViewModel> AllOrders { get; set; }

        public int NumberOfItemsSelected
        {
            get
            {
                return this.AllOrders.Count(vm => vm.IsSelected);
            }
        }

        public bool IsOrderPending
        {
            get
            {
                return this.AllOrders.SingleOrDefault(o => o.IsSelected).Order.Status == OrderStatus.Pending;
            }
        }

        public void AddPropertyChangedEvent(List<OrderViewModel> orders)
        {
            orders.ForEach(ovm => ovm.PropertyChanged += this.OnOrderViewModelPropertyChanged);
        }

        protected override void CreateCommands()
        {
            if (this.customer != null)
            {
                this.Commands.Add(new CommandViewModel("New...", new DelegateCommand(param => this.CreateNewOrderExecute())));
                this.Commands.Add(new CommandViewModel("Edit...", new DelegateCommand(param => this.EditOrderExecute(), p => this.NumberOfItemsSelected == 1 && this.IsOrderPending)));
                this.Commands.Add(new CommandViewModel("Delete", new DelegateCommand(param => this.DeleteOrderExecute(), p => this.NumberOfItemsSelected ==1 && this.IsOrderPending)));
                this.Commands.Add(new CommandViewModel("Place", new DelegateCommand(p => this.PlaceOrder(), p => this.NumberOfItemsSelected == 1 && this.IsOrderPending)));
            }
        }

        private void PlaceOrder()
        {
            OrderViewModel ovm = this.AllOrders.SingleOrDefault(o => o.IsSelected);

            if (ovm != null)
            {
                ovm.Order.Post();
                ovm.UpdateOrderTotals();
            }
        }

        private void OnOrderAdded(object sender, OrderEventArgs e)
        {
            OrderViewModel vm = new OrderViewModel(e.Order, this.repository);
            vm.PropertyChanged += this.OnOrderViewModelPropertyChanged;

            this.AllOrders.Add(vm);
        }

        private void OnOrderViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string isSelected = "IsSelected";

            if (e.PropertyName == isSelected)
            {
                this.OnPropertyChanged("NumberOfItemsSelected");
            }
        }

        private void CreateNewOrderExecute()
        {
            Order order = new Order { Customer = this.customer };

            OrderViewModel viewModel = new OrderViewModel(order, this.repository);

            this.ShowOrder(viewModel);
        }

        private void EditOrderExecute()
        {
            OrderViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                this.ShowOrder(viewModel);

                this.repository.SaveToDatabase();

                viewModel.Order.CalculateTotals();

                viewModel.UpdateOrderTotals();
            }
            else
            {
                MessageBox.Show("Please select only one order.");
            }
        }

        private void DeleteOrderExecute()
        {
            OrderViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                if (MessageBox.Show("Do you really want to delete the selected order?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.repository.RemoveOrder(viewModel.Order);
                    this.repository.SaveToDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select only one order");
            }
        }

        private OrderViewModel GetOnlySelectedViewModel()
        {
            OrderViewModel result;

            try
            {
                result = this.AllOrders.Single(vm => vm.IsSelected);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Creates a new window to edit an order.
        /// </summary>
        /// <param name="viewModel">The view model for the order to be edited.</param>
        private void ShowOrder(OrderViewModel viewModel)
        {
            WorkspaceWindow window = new WorkspaceWindow();
            window.Width = 400;
            viewModel.CloseAction = b => window.DialogResult = b;
            window.Title = viewModel.DisplayName;

            OrderView view = new OrderView();

            view.DataContext = viewModel;

            window.Content = view;

            window.ShowDialog();
        }

        private void OnOrderRemoved(object sender, OrderEventArgs e)
        {
            OrderViewModel viewModel = this.GetOnlySelectedViewModel();
            if (viewModel != null)
            {
                if (viewModel.Order == e.Order)
                {
                    this.AllOrders.Remove(viewModel);
                }
            }
        }
    }
}