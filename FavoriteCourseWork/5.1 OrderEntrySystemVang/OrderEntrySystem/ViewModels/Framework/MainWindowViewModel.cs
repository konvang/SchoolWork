using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using OrderEntryDataAccess;

namespace OrderEntrySystem
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        private ObservableCollection<WorkspaceViewModel> viewModels;

        private Repository repository;

        public MainWindowViewModel()
            : base("Order Entry System - Vang")
        {
            this.repository = new Repository();
        }

        public ObservableCollection<WorkspaceViewModel> ViewModels
        {
            get
            {
                if (this.viewModels == null)
                {
                    this.viewModels = new ObservableCollection<WorkspaceViewModel>();
                }

                return this.viewModels;
            }
        }

        /// <summary>
        /// Creates the commands required by the view model.
        /// </summary>
        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("View all products", new DelegateCommand(p => this.ShowAllProducts())));
            this.Commands.Add(new CommandViewModel("View all customers", new DelegateCommand(p => this.ShowAllCustomers())));
            this.Commands.Add(new CommandViewModel("View all locations", new DelegateCommand(p => this.ShowAllLocations())));
            this.Commands.Add(new CommandViewModel("View all categories", new DelegateCommand(p => this.ShowAllProductCategories())));
            this.Commands.Add(new CommandViewModel("View all orders", new DelegateCommand(p => this.ShowAllOrders())));
            this.Commands.Add(new CommandViewModel("View reports", new DelegateCommand(p => this.ShowReport())));
        }

        private void ShowAllProducts()
        {
            MultiProductViewModel viewModel = this.ViewModels.FirstOrDefault(vm => vm is MultiProductViewModel) as MultiProductViewModel;

            if (viewModel == null)
            {
                viewModel = new MultiProductViewModel(this.repository);

                viewModel.RequestClose += this.OnWorkspaceRequestClose;

                this.ViewModels.Add(viewModel);
            }

            this.ActivateViewModel(viewModel);
        }

        private void ShowAllCustomers()
        {
            MultiCustomerViewModel viewModel = this.ViewModels.FirstOrDefault(vm => vm is MultiCustomerViewModel) as MultiCustomerViewModel;

            if (viewModel == null)
            {
                viewModel = new MultiCustomerViewModel(this.repository);

                viewModel.RequestClose += this.OnWorkspaceRequestClose;

                this.ViewModels.Add(viewModel);
            }

            this.ActivateViewModel(viewModel);
        }

        private void ShowAllLocations()
        {
            MultiLocationViewModel viewModel = this.ViewModels.FirstOrDefault(vm => vm is MultiLocationViewModel) as MultiLocationViewModel;

            if (viewModel == null)
            {
                viewModel = new MultiLocationViewModel(this.repository);

                viewModel.RequestClose += this.OnWorkspaceRequestClose;

                this.ViewModels.Add(viewModel);
            }

            this.ActivateViewModel(viewModel);
        }

        private void ShowAllProductCategories()
        {
            MultiCategoryViewModel viewModel = this.ViewModels.FirstOrDefault(vm => vm is MultiCategoryViewModel) as MultiCategoryViewModel;

            if (viewModel == null)
            {
                viewModel = new MultiCategoryViewModel(this.repository, null);

                viewModel.RequestClose += this.OnWorkspaceRequestClose;

                this.ViewModels.Add(viewModel);
            }

            this.ActivateViewModel(viewModel);
        }

        private void ShowAllOrders()
        {
            MultiOrderViewModel viewModel = this.ViewModels.FirstOrDefault(vm => vm is MultiOrderViewModel) as MultiOrderViewModel;

            if (viewModel == null)
            {
                viewModel = new MultiOrderViewModel(this.repository, null);

                viewModel.RequestClose += this.OnWorkspaceRequestClose;

                this.ViewModels.Add(viewModel);
            }

            this.ActivateViewModel(viewModel);
        }

        private void ShowReport()
        {
            ReportViewModel viewModel = this.ViewModels.FirstOrDefault(vm => vm is ReportViewModel) as ReportViewModel;

            if (viewModel == null)
            {
                viewModel = new ReportViewModel(this.repository);

                viewModel.RequestClose += this.OnWorkspaceRequestClose;

                this.ViewModels.Add(viewModel);
            }

            this.ActivateViewModel(viewModel);
        }

        /// <summary>
        /// A handler which responds to a request to close a workspace.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel viewModel = sender as WorkspaceViewModel;

            this.ViewModels.Remove(viewModel);
        }

        private void ActivateViewModel(WorkspaceViewModel viewModel)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.ViewModels);

            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(viewModel);
            }
        }
    }
}