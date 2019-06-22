using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using OrderEntryDataAccess;
using OrderEntryEngine;

namespace OrderEntrySystem
{
    public class OrderLineViewModel : EntityViewModel<OrderLine>, IDataErrorInfo
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="line">The line to be shown.</param>
        /// <param name="repository">The car repository.</param>
        public OrderLineViewModel(OrderLine line)
            : base("New order line", line)
        {
            this.Entity = line;

            this.ProductRepository = (Repository<Game>)RepositoryManager.GetRepository(typeof(Game));
            this.OrderLineRepositry = (Repository<OrderLine>)RepositoryManager.GetRepository(typeof(OrderLine));
        }

        public Repository<Game> ProductRepository { get; private set; }
        public Repository<OrderLine> OrderLineRepositry { get; private set; }

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

        public OrderLine Line
        {
            get
            {
                return this.Entity;
            }
        }

        [EntityColumn(1, 75)]
        [EntityControl(ControlType.ComboBox, 1)]
        public Game Product
        {
            get
            {
                return this.Entity.Product;
            }
            set
            {
                this.Entity.Product = value;
                this.OnPropertyChanged("Product");
                this.Entity.ProductAmount = value.Price;
                this.OnPropertyChanged("ProductTotal");
                this.Entity.CalculateTax();
                this.OnPropertyChanged("TaxTotal");
            }
        }

        public IEnumerable<Game> Products
        {
            get
            {
                return (RepositoryManager.GetRepository(typeof(Game)) as Repository<Game>).GetEntities();
            }
        }

        public int Quantity
        {
            get
            {
                return this.Entity.Quantity;
            }
            set
            {
                this.Entity.Quantity = value;
                this.OnPropertyChanged("Quantity");
                this.OnPropertyChanged("ProductTotal");
                this.Entity.CalculateTax();
                this.OnPropertyChanged("TaxTotal");
            }
        }

        public decimal ProductTotal
        {
            get
            {
                return this.Entity.ExtendedProductAmount;
            }
        }

        public decimal TaxTotal
        {
            get
            {
                return this.Entity.ExtendedTax;
            }
        }
    }
}