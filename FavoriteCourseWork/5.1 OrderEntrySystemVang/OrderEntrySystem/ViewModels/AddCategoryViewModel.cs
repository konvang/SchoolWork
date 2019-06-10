using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class AddCategoryViewModel : WorkspaceViewModel
    {
        private Repository repository;

        private Product product;

        public AddCategoryViewModel(Repository repository, Product product)
            : base("Add category")
        {
            this.repository = repository;
            this.product = product;
        }

        public Category Category { get; set; }

        public IEnumerable<Category> Categories
        {
            get
            {
                return this.repository.GetCategories();
            }
        }

        /// <summary>
        /// Creates the commands needed for the add category view model.
        /// </summary>
        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("OK", new DelegateCommand(p => this.OkExecute()), true, false));
            this.Commands.Add(new CommandViewModel("Cancel", new DelegateCommand(p => this.CancelExecute()), false, true));
        }

        private void Save()
        {
            ProductCategory pc = new ProductCategory();

            pc.Category = this.Category;
            pc.Product = this.product;

            this.repository.AddProductCategory(pc);

            this.repository.SaveToDatabase();
        }

        private void OkExecute()
        {
            this.Save();
            this.CloseAction(true);
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