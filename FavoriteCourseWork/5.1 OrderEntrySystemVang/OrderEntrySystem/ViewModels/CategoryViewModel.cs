using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class CategoryViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        private Category category;

        private Repository repository;

        private bool isSelected;

        public CategoryViewModel(Category category, Repository repository)
            : base("New product category")
        {
            this.category = category;
            this.repository = repository;
        }

        public string Error
        {
            get
            {
                return this.category.Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.category[propertyName];
            }
        }

        public Category Category
        {
            get
            {
                return this.category;
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
                return this.category.Name;
            }
            set
            {
                this.category.Name = value;
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Creates the commands needed for the product category view model.
        /// </summary>
        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("OK", new DelegateCommand(p => this.OkExecute()), true, false));
            this.Commands.Add(new CommandViewModel("Cancel", new DelegateCommand(p => this.CancelExecute()), false, true));
        }

        private bool Save()
        {
            bool result = true;

            if (this.Category.IsValid)
            {
                // Add product category to repository.
                this.repository.AddCategory(this.category);

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("One or more properties are invalid. Customer could not be saved.");
                result = false;
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
        /// Closes the window without saving.
        /// </summary>
        private void CancelExecute()
        {
            this.CloseAction(false);
        }
    }
}