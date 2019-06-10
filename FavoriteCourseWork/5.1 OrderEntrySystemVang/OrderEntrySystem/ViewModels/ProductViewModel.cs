using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class ProductViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        private Product product;

        private Repository repository;

        private bool isSelected;

        private MultiCategoryViewModel filteredCategoryViewModel;

        public ProductViewModel(Product product, Repository repository)
            : base("New product")
        {
            this.product = product;
            this.repository = repository;
            this.filteredCategoryViewModel = new MultiCategoryViewModel(this.repository, this.product);
            this.filteredCategoryViewModel.AllCategories = this.FilteredCategories;
        }

        public string Error
        {
            get
            {
                return this.product.Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.product[propertyName];
            }
        }

        public MultiCategoryViewModel FilteredCategoryViewModel
        {
            get
            {
                return this.filteredCategoryViewModel;
            }
        }

        public ObservableCollection<CategoryViewModel> FilteredCategories
        {
            get
            {
                List<CategoryViewModel> categories = null;

                if (this.product.ProductCategories != null)
                {
                    categories =
                        (from c in this.product.ProductCategories
                        select new CategoryViewModel(c.Category, this.repository)).ToList();
                }

                this.FilteredCategoryViewModel.AddPropertyChangedEvent(categories);

                return new ObservableCollection<CategoryViewModel>(categories);
            }
        }

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

        public string Name
        {
            get
            {
                return this.product.Name;
            }
            set
            {
                this.product.Name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public OrderEntryEngine.Condition Condition
        {
            get
            {
                return this.product.Condition;
            }
            set
            {
                this.product.Condition = value;
                this.OnPropertyChanged("Condition");
            }
        }

        public ICollection<OrderEntryEngine.Condition> Conditions
        {
            get
            {
                return Enum.GetValues(typeof(OrderEntryEngine.Condition)) as ICollection<OrderEntryEngine.Condition>;
            }
        }

        public string Description
        {
            get
            {
                return this.product.Description;
            }
            set
            {
                this.product.Description = value;
                this.OnPropertyChanged("Description");
            }
        }

        public Product Product
        {
            get
            {
                return this.product;
            }
        }

        public decimal Price
        {
            get
            {
                return this.product.Price;
            }
            set
            {
                this.product.Price = value;
                this.OnPropertyChanged("Price");
            }
        }

        public Location Location
        {
            get
            {
                return this.product.Location;
            }
            set
            {
                this.product.Location = value;
                this.OnPropertyChanged("Location");
            }
        }

        public int Quantity
        {
            get
            {
                return this.product.Quantity;
            }
            set
            {
                this.product.Quantity = value;
            }
        }

        public ICollection<Location> Locations
        {
            get
            {
                return this.repository.GetLocations();
            }
        }

        public IEnumerable<Category> Categories
        {
            get
            {
                return this.repository.GetCategories() as IEnumerable<Category>;
            }
        }

        /// <summary>
        /// Creates the commands needed for the product view model.
        /// </summary>
        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("OK", new DelegateCommand(p => this.OkExecute()), true, false));
            this.Commands.Add(new CommandViewModel("Cancel", new DelegateCommand(p => this.CancelExecute()), false, true));
        }

        private bool Save()
        {
            bool result = true;

            if (this.Product.IsValid)
            {
                // Add product to repository.
                this.repository.AddProduct(this.product);

                this.repository.SaveToDatabase();
            }
            else
            {
                result = false;
                MessageBox.Show("One or more fields are invalid. The product could not be saved.");
            }

            return result;
        }

        private void OkExecute()
        {
            if (this.Save())
            {
                this.CloseAction(true);
            }
        }

        /// <summary>
        /// Closes the new Item window without saving.
        /// </summary>
        private void CancelExecute()
        {
            this.CloseAction(false);
        }
    }
}