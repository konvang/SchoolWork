using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class MultiCategoryViewModel : WorkspaceViewModel
    {
        private Repository repository;

        private Product product;

        public MultiCategoryViewModel(Repository repository, Product product)
            : base("All categories")
        {
            this.repository = repository;
            this.product = product;

            this.Commands.Clear();
            this.CreateCommands();

            List<CategoryViewModel> categories =
                (from category in this.repository.GetCategories()
                 select new CategoryViewModel(category, this.repository)).ToList();

            this.AddPropertyChangedEvent(categories);

            this.AllCategories = new ObservableCollection<CategoryViewModel>(categories);

            this.repository.CategoryAdded += this.OnCategoryAdded;
            this.repository.CategoryRemoved += this.OnCategoryRemoved;
        }

        public ObservableCollection<CategoryViewModel> AllCategories { get; set; }

        public int NumberOfItemsSelected
        {
            get
            {
                return this.AllCategories.Count(vm => vm.IsSelected);
            }
        }

        public void AddPropertyChangedEvent(List<CategoryViewModel> categories)
        {
            categories.ForEach(cvm => cvm.PropertyChanged += this.OnCategoryViewModelPropertyChanged);
        }

        protected override void CreateCommands()
        {
            if (this.product == null)
            {
                this.Commands.Add(new CommandViewModel("New...", new DelegateCommand(param => this.CreateNewCategoryExecute())));
                this.Commands.Add(new CommandViewModel("Edit...", new DelegateCommand(param => this.EditCategoryExecute(), p => this.NumberOfItemsSelected == 1)));
                this.Commands.Add(new CommandViewModel("Delete", new DelegateCommand(param => this.DeleteCategoryExecute(), p => this.NumberOfItemsSelected == 1)));
            }
            else
            {
                this.Commands.Add(new CommandViewModel("Add...", new DelegateCommand(param => this.AddCategoryExecute())));
                this.Commands.Add(new CommandViewModel("Remove", new DelegateCommand(param => this.RemoveCategoryExecute(), p => this.NumberOfItemsSelected == 1)));
            }
        }

        private void RemoveCategoryExecute()
        {
            CategoryViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                if (MessageBox.Show("Are you sure you want to remove selected category?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ProductCategory productCategory = null;

                    foreach (ProductCategory pc in this.product.ProductCategories)
                    {
                        if (pc.Category == viewModel.Category)
                        {
                            productCategory = pc;
                            break;
                        }
                    }

                    this.repository.RemoveProductCategory(productCategory);
                    this.repository.SaveToDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select a car from the list.");
            }
        }

        private void AddCategoryExecute()
        {
            AddCategoryViewModel viewModel = new AddCategoryViewModel(this.repository, this.product);

            this.ShowCategory(viewModel, new AddCategoryView());
        }

        private void OnCategoryAdded(object sender, CategoryEventArgs e)
        {
            CategoryViewModel vm = new CategoryViewModel(e.Category, this.repository);
            vm.PropertyChanged += this.OnCategoryViewModelPropertyChanged;

            this.AllCategories.Add(vm);
        }

        private void OnCategoryRemoved(object sender, CategoryEventArgs e)
        {
            CategoryViewModel viewModel = this.AllCategories.FirstOrDefault(vm => vm.Category == e.Category);

            if (viewModel != null)
            {
                if (viewModel.Category == e.Category)
                {
                    this.AllCategories.Remove(viewModel);
                }
            }
        }

        /// <summary>
        /// A handler which responds when a location view model's property changes.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnCategoryViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string isSelected = "IsSelected";

            if (e.PropertyName == isSelected)
            {
                this.OnPropertyChanged("NumberOfItemsSelected");
            }
        }

        private void CreateNewCategoryExecute()
        {
            Category category = new Category();

            CategoryViewModel viewModel = new CategoryViewModel(category, this.repository);

            this.ShowCategory(viewModel, new CategoryView());
        }

        private void EditCategoryExecute()
        {
            CategoryViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                this.ShowCategory(viewModel, new CategoryView());

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("Please select only one category.");
            }
        }

        private void DeleteCategoryExecute()
        {
            CategoryViewModel viewModel = this.GetOnlySelectedViewModel();

            if (viewModel != null)
            {
                if (MessageBox.Show("Do you really want to delete the selected category?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.repository.RemoveCategory(viewModel.Category);
                    this.repository.SaveToDatabase();
                }
            }
            else
            {
                MessageBox.Show("Please select only one category.");
            }
        }

        private CategoryViewModel GetOnlySelectedViewModel()
        {
            CategoryViewModel result;

            try
            {
                result = this.AllCategories.Single(vm => vm.IsSelected);
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
        /// <param name="view">The view for the category to be edited.</param>
        private void ShowCategory(WorkspaceViewModel viewModel, UserControl view)
        {
            WorkspaceWindow window = new WorkspaceWindow();
            window.Width = 400;
            viewModel.CloseAction = b => window.DialogResult = b;
            window.Title = viewModel.DisplayName;

            view.DataContext = viewModel;

            window.Content = view;

            window.ShowDialog();
        }
    }
}