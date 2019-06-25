using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Input;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class ProductViewModel : EntityViewModel<Game>, IDataErrorInfo
    {
        private MultiEntityViewModel<Category, CategoryViewModel, EntityView> filteredCategoryViewModel;

        public ProductViewModel(Game product)
            : base("New product", product)
        {
            this.Entity = product;
            this.filteredCategoryViewModel = new MultiEntityViewModel<Category, CategoryViewModel, EntityView>();
            this.filteredCategoryViewModel.AllEntities = this.FilteredCategories;
            this.ProductRepo = (Repository<Game>)RepositoryManager.GetRepository(typeof(Game));
        }

        public Repository<Game> ProductRepo { get; private set; }

        public string Error
        {
            get
            {
                return this.Entity.Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.Entity[propertyName];
            }
        }

        public MultiEntityViewModel<Category, CategoryViewModel, EntityView> FilteredCategoryViewModel
        {
            get
            {
                return this.filteredCategoryViewModel;
            }
        }

        [EntityControl(ControlType.Button, "Export", 14)]
        public ICommand Export
        {
            get
            {
                return new DelegateCommand(p => this.ExportExecute());
            }
        }

        public void ExportExecute()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"C:\temp\Games.txt", FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, this.Entity);
            stream.Close();
        }

        public ObservableCollection<CategoryViewModel> FilteredCategories
        {
            get
            {
                List<CategoryViewModel> categories = null;

                if (this.Entity.ProductCategories != null)
                {
                    categories =
                        (from c in this.Entity.ProductCategories
                        select new CategoryViewModel(c.Category)).ToList();
                }

                this.FilteredCategoryViewModel.AddPropertyChangedEvent(categories);

                return new ObservableCollection<CategoryViewModel>(categories);
            }
        }

        [EntityColumn(1, 75)]
        [EntityControl(ControlType.Label, 1)]
        public decimal ExtendedPrice
        {
            get
            {
                return this.Entity.ExtendedPrice;
            }
        }

        [EntityColumn(2, 75)]
        [EntityControlAttribute(ControlType.TextBox, 2)]
        public string Name
        {
            get
            {
                return this.Entity.Name;
            }
            set
            {
                this.Entity.Name = value;
                this.OnPropertyChanged("Name");
            }
        }

        [EntityColumn(3, 75)]
        [EntityControlAttribute(ControlType.ComboBox, 3)]
        public OrderEntryEngine.Condition Condition
        {
            get
            {
                return this.Entity.Condition;
            }
            set
            {
                this.Entity.Condition = value;
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

        [EntityColumn(4, 75)]
        [EntityControlAttribute(ControlType.TextBox, 4)]
        public string Description
        {
            get
            {
                return this.Entity.Description;
            }
            set
            {
                this.Entity.Description = value;
                this.OnPropertyChanged("Description");
            }
        }

        [EntityColumn(5, 75)]
        [EntityControlAttribute(ControlType.ComboBox, 5)]
        public Game Product
        {
            get
            {
                return this.Entity;
            }
        }

        [EntityColumn(6, 75)]
        [EntityControlAttribute(ControlType.TextBox, 6)]
        public decimal Price
        {
            get
            {
                return this.Entity.Price;
            }
            set
            {
                this.Entity.Price = value;
                this.OnPropertyChanged("Price");
            }
        }

        [EntityColumn(7, 75)]
        [EntityControlAttribute(ControlType.ComboBox, 7)]
        public Location Location
        {
            get
            {
                return this.Entity.Location;
            }
            set
            {
                this.Entity.Location = value;
                this.OnPropertyChanged("Location");
            }
        }

        [EntityColumn(8, 75)]
        [EntityControlAttribute(ControlType.TextBox, 8)]
        public int Quantity
        {
            get
            {
                return this.Entity.Quantity;
            }
            set
            {
                this.Entity.Quantity = value;
            }
        }

        public ICollection<Location> Locations
        {
            get
            {
                return (RepositoryManager.GetRepository(typeof(Location)) as Repository<Location>).GetEntities();
            }
        }

        public IEnumerable<Category> Categories
        {
            get
            {
                return (RepositoryManager.GetRepository(typeof(Category)) as Repository<Category>).GetEntities();
            }
        }
    }
}