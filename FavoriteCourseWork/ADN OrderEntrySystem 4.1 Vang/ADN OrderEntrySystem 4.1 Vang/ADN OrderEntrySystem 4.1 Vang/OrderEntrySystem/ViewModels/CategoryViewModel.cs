using System.Windows;
using System.ComponentModel;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class CategoryViewModel : EntityViewModel<Category>, IDataErrorInfo
    {
        public CategoryViewModel(Category category)
            : base("New product category", category)
        {
            this.Entity = category;
            this.CategoryRepository = (Repository<Category>)RepositoryManager.GetRepository(typeof(Category));
        }

        public Repository<Category> CategoryRepository { get; set; }

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

        public Category Category
        {
            get
            {
                return this.Entity;
            }
        }

        [EntityColumn(1, 75)]
        [EntityControl(ControlType.TextBox, 1)]
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
    }
}