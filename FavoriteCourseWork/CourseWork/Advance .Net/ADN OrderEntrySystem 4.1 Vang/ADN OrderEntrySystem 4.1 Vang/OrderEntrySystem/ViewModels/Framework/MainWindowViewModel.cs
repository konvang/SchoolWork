using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        private ObservableCollection<UserControl> views;

        public MainWindowViewModel()
            : base("Order Entry System - Vang")
        {
            RepositoryManager.Context = new OrderEntryContext();
            RepositoryManager.IntitializeRepository(new OrderEntryContext());
        }

        public ObservableCollection<UserControl> Views
        {
            get
            {
                if (this.views == null)
                {
                    this.views = new ObservableCollection<UserControl>();
                }

                return this.views;
            }
        }

        /// <summary>
        /// Creates the commands required by the view model.
        /// </summary>
        protected override void CreateCommands()
        {
            this.AddMultiEntityCommand("View all products");
            this.AddMultiEntityCommand("View all customers");
            this.AddMultiEntityCommand("View all locations");
            this.AddMultiEntityCommand("View all categories");
            this.AddMultiEntityCommand("View all orders");
        }

        private void ShowAllEntities(string displayName)
        {
            //displayName.FirstOrDefault();

            UserControl view = this.Views.FirstOrDefault(v => (v.DataContext as IMultiEntityViewModel).DisplayName == displayName);

            WorkspaceViewModel viewModel = null;

            if (view == null)
            {
                switch (displayName)
                {
                    case "View all products":
                        view = new MultiEntityView();
                        viewModel = new MultiEntityViewModel<Game, ProductViewModel, EntityView>();
                        break;

                    case "View all customers":
                        view = new MultiEntityView();
                        viewModel = new MultiEntityViewModel<Customer, CustomerViewModel, EntityView>();
                        break;

                    case "View all locations":
                        view = new MultiEntityView();
                        viewModel = new MultiEntityViewModel<Location, LocationViewModel, EntityView>();
                        break;

                    case "View all categories":
                        view = new MultiEntityView();
                        viewModel = new MultiEntityViewModel<Category, CategoryViewModel, EntityView>();
                        break;

                    case "View all orders":
                        view = new MultiEntityView();
                        viewModel = new MultiEntityViewModel<Order, OrderViewModel, EntityView>();
                        break;
                }

                viewModel.RequestClose += this.OnWorkspaceRequestClose;

                view.DataContext = viewModel;

                this.Views.Add(view);
            }

            this.ActivateViewModel(view);
        }

        /// <summary>
        /// A handler which responds to a request to close a workspace.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The arguments for the event.</param>
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            UserControl viewModel = sender as UserControl;

            UserControl view = this.Views.FirstOrDefault(uc => uc.DataContext == viewModel);

            this.Views.Remove(view);
        }

        private void ActivateViewModel(UserControl viewModel)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Views);

            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(viewModel);
            }
        }

        private void AddMultiEntityCommand(string displayName)
        {
            this.Commands.Add(new CommandViewModel(displayName, new DelegateCommand(p => this.ShowAllEntities(displayName))));
        }
    }
}