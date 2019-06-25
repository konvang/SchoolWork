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
     

        private Order order;

        public MultiOrderLineViewModel(Order order)
            : base("All lines")
        {
          
            this.order = order;

            List<OrderLineViewModel> lines =
                (from line in (RepositoryManager.GetRepository(typeof(OrderLine)) as Repository<OrderLine>).GetEntities()
                 select new OrderLineViewModel(line)).ToList();

            this.AddPropertyChangedEvent(lines);

            this.AllLines = new ObservableCollection<OrderLineViewModel>(lines);

            (RepositoryManager.GetRepository(typeof(OrderLine)) as Repository<OrderLine>).EntityAdded += this.OnOrderLineAdded;
            (RepositoryManager.GetRepository(typeof(OrderLine)) as Repository<OrderLine>).EntityRemoved += this.OnOrderLineRemoved;
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

        private void OnOrderLineAdded(object sender, EntityEventArgs<OrderLine> e)
        {
            OrderLineViewModel vm = new OrderLineViewModel(e.Entity);
            vm.PropertyChanged += this.OnOrderLineViewModelPropertyChanged;

            this.AllLines.Add(vm);
        }

        private void OnOrderLineRemoved(object sender, EntityEventArgs<OrderLine> e)
        {
            OrderLineViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                if (viewModel.Line == e.Entity)
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

            OrderLineViewModel viewModel = new OrderLineViewModel(category);

            this.ShowOrderLine(viewModel);
        }

        private void EditOrderLineExecute()
        {
            OrderLineViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                this.ShowOrderLine(viewModel);

                RepositoryManager.Context.SaveChanges();
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

                    (RepositoryManager.GetRepository(typeof(OrderLine)) as Repository<OrderLine>).RemoveEntity(viewModel.Line);
                    (RepositoryManager.GetRepository(typeof(OrderLine)) as Repository<OrderLine>).SaveToDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select only one line.");
            }
        }

        public Repository<OrderLine> OrderLineRepository
        {
            get
            {
                return (Repository<OrderLine>)RepositoryManager.GetRepository(typeof(OrderLine));
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

            EntityView view = new EntityView();

            view.DataContext = viewModel;

            window.Content = view;

            window.ShowDialog();
        }
    }
}