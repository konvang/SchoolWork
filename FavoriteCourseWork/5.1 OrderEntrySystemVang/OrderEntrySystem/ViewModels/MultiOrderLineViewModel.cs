using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class MultiOrderLineViewModel : WorkspaceViewModel
    {
        private Repository repository;

        private Order order;

        public MultiOrderLineViewModel(Repository repository, Order order)
            : base("All lines")
        {
            this.repository = repository;
            this.order = order;

            List<OrderLineViewModel> lines =
                (from line in this.order.Lines
                 select new OrderLineViewModel(line, this.repository)).ToList();

            this.AddPropertyChangedEvent(lines);

            this.AllLines = new ObservableCollection<OrderLineViewModel>(lines);

            this.repository.OrderLineAdded += this.OnOrderLineAdded;
            this.repository.OrderLineRemoved += this.OnOrderLineRemoved;
        }

        public ObservableCollection<OrderLineViewModel> AllLines { get; set; }

        public int NumberOfItemsSelected
        {
            get
            {
                return this.AllLines.Count(vm => vm.IsSelected);
            }
        }

        public void AddPropertyChangedEvent(List<OrderLineViewModel> lines)
        {
            lines.ForEach(lvm => lvm.PropertyChanged += this.OnOrderLineViewModelPropertyChanged);
        }

        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("New...", new DelegateCommand(param => this.CreateNewOrderLineExecute())));
            this.Commands.Add(new CommandViewModel("Edit...", new DelegateCommand(param => this.EditOrderLineExecute(), p => this.NumberOfItemsSelected == 1)));
            this.Commands.Add(new CommandViewModel("Delete", new DelegateCommand(param => this.DeleteOrderLineExecute(), p => this.NumberOfItemsSelected == 1)));
        }

        private void OnOrderLineAdded(object sender, OrderLineEventArgs e)
        {
            OrderLineViewModel vm = new OrderLineViewModel(e.Line, this.repository);
            vm.PropertyChanged += this.OnOrderLineViewModelPropertyChanged;

            this.AllLines.Add(vm);
        }

        private void OnOrderLineRemoved(object sender, OrderLineEventArgs e)
        {
            OrderLineViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                if (viewModel.Line == e.Line)
                {
                    this.AllLines.Remove(viewModel);
                }
            }
        }

        /// <summary>
        /// A handler which responds when a location view model's property changes.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnOrderLineViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string isSelected = "IsSelected";

            if (e.PropertyName == isSelected)
            {
                this.OnPropertyChanged("NumberOfItemsSelected");
            }
        }

        private void CreateNewOrderLineExecute()
        {
            OrderLine category = new OrderLine { Order = order, Quantity = 1 };

            OrderLineViewModel viewModel = new OrderLineViewModel(category, this.repository);

            this.ShowOrderLine(viewModel);
        }

        private void EditOrderLineExecute()
        {
            OrderLineViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                this.ShowOrderLine(viewModel);

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("Please select only one order line.");
            }
        }

        private void DeleteOrderLineExecute()
        {
            OrderLineViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                if (MessageBox.Show("Do you really want to delete the selected line?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.repository.RemoveLine(viewModel.Line);
                    this.repository.SaveToDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select only one line.");
            }
        }

        private OrderLineViewModel GetOnlySelectedViewModel()
        {
            OrderLineViewModel result;

            try
            {
                result = this.AllLines.Single(vm => vm.IsSelected);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Creates a new window to edit a category.
        /// </summary>
        /// <param name="viewModel">The view model for the category to be edited.</param>
        private void ShowOrderLine(OrderLineViewModel viewModel)
        {
            WorkspaceWindow window = new WorkspaceWindow();
            window.Width = 400;
            viewModel.CloseAction = b => window.DialogResult = b;
            window.Title = viewModel.DisplayName;

            OrderLineView view = new OrderLineView();

            view.DataContext = viewModel;

            window.Content = view;

            window.ShowDialog();
        }
    }
}