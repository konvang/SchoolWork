using System.Collections.Generic;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class AddCategoryViewModel : EntityViewModel<Game>
    {
        public AddCategoryViewModel( Game product)
            : base("Add category", product)
        { 
            this.Entity = product;
        }

        public Category Category { get; set; }

        public IEnumerable<Category> Categories
        {
            get
            {
              return (RepositoryManager.GetRepository(typeof(Category)) as Repository<Category>).GetEntities();
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

        new private void Save()
        {
            ProductCategory pc = new ProductCategory();

            pc.Category = this.Category;
            pc.Product = this.Entity;
            
            (RepositoryManager.GetRepository(typeof(Category)) as Repository<Category>).AddEntity(pc.Category);
            (RepositoryManager.GetRepository(typeof(Category)) as Repository<Category>).SaveToDatabase();
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