using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class OrderLineViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        private OrderLine line;

        /// <summary>
        /// The car view model's database repository.
        /// </summary>
        private Repository repository;

        private bool isSelected;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="line">The line to be shown.</param>
        /// <param name="repository">The car repository.</param>
        public OrderLineViewModel(OrderLine line, Repository repository)
            : base("New order line")
        {
            this.line = line;
            this.repository = repository;
        }

        public string Error
        {
            get
            {
                return this.line.Error;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.line[propertyName];
            }
        }

        public OrderLine Line
        {
            get
            {
                return this.line;
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

        public Product Product
        {
            get
            {
                return this.line.Product;
            }
            set
            {
                this.line.Product = value;
                this.OnPropertyChanged("Product");
                this.line.ProductAmount = value.Price;
                this.OnPropertyChanged("ProductTotal");
                this.line.CalculateTax();
                this.OnPropertyChanged("TaxTotal");
            }
        }

        public IEnumerable<Product> Products
        {
            get
            {
                return this.repository.GetProducts();
            }
        }

        public int Quantity
        {
            get
            {
                return this.line.Quantity;
            }
            set
            {
                this.line.Quantity = value;
                this.OnPropertyChanged("Quantity");
                this.OnPropertyChanged("ProductTotal");
                this.line.CalculateTax();
                this.OnPropertyChanged("TaxTotal");
            }
        }

        public decimal ProductTotal
        {
            get
            {
                return this.line.ExtendedProductAmount;
            }
        }

        public decimal TaxTotal
        {
            get
            {
                return this.line.ExtendedTax;
            }
        }

        /// <summary>
        /// Creates the commands needed for the car view model.
        /// </summary>
        protected override void CreateCommands()
        {
            this.Commands.Add(new CommandViewModel("OK", new DelegateCommand(p => this.OkExecute()), true, false));
            this.Commands.Add(new CommandViewModel("Cancel", new DelegateCommand(p => this.CancelExecute()), false, true));
        }

        /// <summary>
        /// Saves the car view model's car to the repository.
        /// </summary>
        private bool Save()
        {
            bool result = true;

            if (this.Line.IsValid)
            {
                // Add line to repository.
                this.repository.AddLine(this.line);

                this.repository.SaveToDatabase();
            }
            else
            {
                MessageBox.Show("One or more properties are invalid. Order line could not be saved.");
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Saves the car and closes the new car window.
        /// </summary>
        private void OkExecute()
        {
            if (this.Save())
            {
                this.CloseAction(true);
            }
        }

        /// <summary>
        /// Closes the new car window without saving.
        /// </summary>
        private void CancelExecute()
        {
            this.CloseAction(false);
        }
    }
}