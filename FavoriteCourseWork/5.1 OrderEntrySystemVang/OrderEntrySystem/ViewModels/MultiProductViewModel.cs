using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class MultiProductViewModel : WorkspaceViewModel
    {
        private Repository repository;

        private ObservableCollection<ProductViewModel> displayedProducts;

        private CollectionViewSource productViewSource;

        /// <summary>
        /// The sort column's name.
        /// </summary>
        private string sortColumnName;

        /// <summary>
        /// The sort direction.
        /// </summary>
        private ListSortDirection sortDirection;

        private List<ProductViewModel> allProductsSorted;

        public MultiProductViewModel(Repository repository)
            : base("All products")
        {
            this.repository = repository;
            this.displayedProducts = new ObservableCollection<ProductViewModel>();

            this.productViewSource = new CollectionViewSource();
            this.productViewSource.Source = this.DisplayedProducts;

            this.SortCommand = new DelegateCommand(this.Sort);

            this.PopulateAllProducts();

            this.repository.ProductAdded += this.OnProductAdded;
            this.repository.ProductRemoved += this.OnProductRemoved;

            this.Pager = new PagingViewModel(this.AllProducts.Count);
            this.Pager.CurrentPageChanged += this.OnPageChanged;

            this.RebuildPageData();
        }

        public ListCollectionView SortedProducts
        {
            get
            {
                return this.productViewSource.View as ListCollectionView;
            }
        }

        public ObservableCollection<ProductViewModel> AllProducts { get; set; }

        public ObservableCollection<ProductViewModel> DisplayedProducts
        {
            get
            {
                return this.displayedProducts;
            }
            private set
            {
                this.displayedProducts = value;

                // Create and link to collection view source.
                this.productViewSource = new CollectionViewSource();
                this.productViewSource.Source = this.DisplayedProducts;
            }
        }

        public PagingViewModel Pager { get; private set; }

        public ICommand SortCommand { get; private set; }

        public int NumberOfItemsSelected
        {
            get
            {
                return this.AllProducts.Count(vm => vm.IsSelected);
            }
        }

        public ObservableCollection<CommandViewModel> FilterCommands { get; private set; }

        public IEnumerable<OrderEntryEngine.Condition> Conditions
        {
            get
            {
                return Enum.GetValues(typeof(OrderEntryEngine.Condition)) as IEnumerable<OrderEntryEngine.Condition>;
            }
        }

        public OrderEntryEngine.Condition FilterCondition { get; set; }

        public string SearchText { get; set; }

        public void AddPropertyChangedEvent(List<ProductViewModel> products)
        {
            products.ForEach(pvm => pvm.PropertyChanged += this.OnProductViewModelPropertyChanged);
        }

        public void RebuildPageData()
        {
            this.DisplayedProducts.Clear();

            int startingIndex = this.Pager.PageSize * (this.Pager.CurrentPage - 1);

            List<ProductViewModel> displayedProducts = this.allProductsSorted.Skip(startingIndex).Take(this.Pager.PageSize).ToList();

            this.Pager.ItemCount = this.AllProducts.Count;

            displayedProducts.ForEach(vm => this.DisplayedProducts.Add(vm));
        }

        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("New...", new DelegateCommand(param => this.CreateNewProductExecute())));
            this.Commands.Add(new CommandViewModel("Edit...", new DelegateCommand(param => this.EditProductExecute(), param => this.NumberOfItemsSelected == 1)));
            this.Commands.Add(new CommandViewModel("Delete", new DelegateCommand(param => this.DeleteProductExecute(), param => this.NumberOfItemsSelected == 1)));

            this.FilterCommands = new ObservableCollection<CommandViewModel>();

            this.FilterCommands.Add(new CommandViewModel("Filter", new DelegateCommand(p => this.Filter())));
            this.FilterCommands.Add(new CommandViewModel("Search", new DelegateCommand(p => this.Search())));
            this.FilterCommands.Add(new CommandViewModel("Clear", new DelegateCommand(p => this.ClearFilters())));
        }

        /// <summary>
        /// Sort the displayed data ascending, or descending.
        /// </summary>
        /// <param name="parameter">The object to sort.</param>
        public void Sort(object parameter)
        {
            string columnName = parameter as string;

            CollectionViewSource sortCollection = new CollectionViewSource();
            sortCollection.Source = this.AllProducts;

            // If clicking on the header of the currently-sorted column...
            if (this.sortColumnName == columnName)
            {
                // Toggle sorting direction.
                this.sortDirection = this.sortDirection == ListSortDirection.Ascending ?
                    ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            else
            {
                // Set the sored column.
                this.sortColumnName = columnName;

                // Set sort direction to ascending.
                this.sortDirection = ListSortDirection.Ascending;
            }

            this.allProductsSorted.Clear();

            // Clear and reset the sort order of the list view.
            sortCollection.SortDescriptions.Clear();
            sortCollection.SortDescriptions.Add(new SortDescription(this.sortColumnName, this.sortDirection));

            var list = sortCollection.View.Cast<ProductViewModel>().ToList();

            list.ForEach(o => this.allProductsSorted.Add(o));

            this.RebuildPageData();
        }

        private void OnPageChanged(object sender, CurrentPageChangeEventArgs e)
        {
            this.RebuildPageData();
        }

        private void Search()
        {
            string searchText = this.SearchText.ToLower();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var filteredProducts =
                    from p in this.repository.GetProducts()
                    where p.Name.ToLower().Contains(searchText) || p.Description.ToLower().Contains(searchText)
                    select new ProductViewModel(p, this.repository);

                this.AllProducts = new ObservableCollection<ProductViewModel>(filteredProducts);
                this.OnPropertyChanged("AllProducts");
                this.allProductsSorted = new List<ProductViewModel>(filteredProducts);
                this.RebuildPageData();
            }
        }

        private void Filter()
        {
            var filteredProducts =
                from p in this.repository.GetProducts()
                where p.Condition == this.FilterCondition
                select new ProductViewModel(p, this.repository);

            this.AllProducts = new ObservableCollection<ProductViewModel>(filteredProducts);
            this.OnPropertyChanged("AllProducts");
            this.allProductsSorted = new List<ProductViewModel>(filteredProducts);
            this.RebuildPageData();
        }

        private void ClearFilters()
        {
            this.FilterCondition = OrderEntryEngine.Condition.Poor;
            this.OnPropertyChanged("ConditionToFilter");
            this.SearchText = string.Empty;
            this.OnPropertyChanged("SearchText");
            this.PopulateAllProducts();
            this.RebuildPageData();
        }

        private void PopulateAllProducts()
        {
            List<ProductViewModel> products =
                (from item in this.repository.GetProducts()
                 select new ProductViewModel(item, this.repository)).ToList();

            this.AddPropertyChangedEvent(products);

            this.AllProducts = new ObservableCollection<ProductViewModel>(products);
            this.OnPropertyChanged("AllProducts");
            this.allProductsSorted = new List<ProductViewModel>(products);
        }

        private void OnProductAdded(object sender, ProductEventArgs e)
        {
            ProductViewModel vm = new ProductViewModel(e.Product, this.repository);
            vm.PropertyChanged += this.OnProductViewModelPropertyChanged;

            this.AllProducts.Add(vm);
        }

        /// <summary>
        /// A handler which responds when a product view model's property changes.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnProductViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string isSelected = "IsSelected";

            if (e.PropertyName == isSelected)
            {
                this.OnPropertyChanged("NumberOfItemsSelected");
            }
        }

        private void CreateNewProductExecute()
        {
            Product product = new Product();

            ProductViewModel viewModel = new ProductViewModel(product, this.repository);

            this.ShowProduct(viewModel);
        }

        private void EditProductExecute()
        {
            ProductViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                this.ShowProduct(viewModel);

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("Please select only one product.");
            }
        }

        private void DeleteProductExecute()
        {
            ProductViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                if (MessageBox.Show("Do you really want to delete the selected Item?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.repository.RemoveProduct(viewModel.Product);
                    this.repository.SaveToDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select only one Item");
            }
        }

        private ProductViewModel GetOnlySelectedViewModel()
        {
            ProductViewModel result;

            try
            {
                result = this.AllProducts.Single(vm => vm.IsSelected);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Creates a new window to edit a car.
        /// </summary>
        /// <param name="viewModel">The view model for the car to be edited.</param>
        private void ShowProduct(WorkspaceViewModel viewModel)
        {
            WorkspaceWindow window = new WorkspaceWindow();
            window.Width = 400;
            viewModel.CloseAction = b => window.DialogResult = b;
            window.Title = viewModel.DisplayName;

            ProductView view = new ProductView();

            view.DataContext = viewModel;

            window.Content = view;

            window.ShowDialog();
        }

        private void OnProductRemoved(object sender, ProductEventArgs e)
        {
            ProductViewModel viewModel = this.GetOnlySelectedViewModel();
            if (viewModel != null)
            {
                if (viewModel.Product == e.Product)
                {
                    this.AllProducts.Remove(viewModel);
                }
            }
        }
    }
}