using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace OrderEntryEngine
{
    public class OrderLine : IDataErrorInfo
    {
        /// <summary>
        /// The property names to validate.
        /// </summary>
        private static readonly string[] propertiesToValidate =
        {
            "Quantity"
        };

        private decimal productAmount;

        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public bool IsArchived { get; set; }

        public decimal ProductAmount
        {
            get
            {
                return this.productAmount;
            }
            set
            {
                this.productAmount = Math.Round(value, 2);
            }
        }

        public decimal ExtendedProductAmount
        {
            get
            {
                return Math.Round(this.Quantity * this.ProductAmount, 2);
            }
        }

        public decimal TaxPerProduct { get; set; }

        public decimal ExtendedTax
        {
            get
            {
                return Math.Round(this.Quantity * this.TaxPerProduct, 2);
            }
        }

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return this.GetValidationError(propertyName);
            }
        }

        public bool IsValid
        {
            get
            {
                bool result = true;

                foreach (string s in OrderLine.propertiesToValidate)
                {
                    if (this.GetValidationError(s) != null)
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }

        public void CalculateTax()
        {
            decimal taxRate = decimal.Parse(ConfigurationManager.AppSettings["TaxRate"]);

            this.TaxPerProduct = this.ProductAmount * taxRate;
        }

        public void Post()
        {
            this.ProductAmount = this.Product.Price;
            this.Product.Quantity -= this.Quantity;
        }

        /// <summary>
        /// Gets the validation error for a specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property to test against.</param>
        /// <returns>The validation error.</returns>
        private string GetValidationError(string propertyName)
        {
            string result = null;

            switch (propertyName)
            {
                case "Quantity":
                    result = this.ValidateQuantity();
                    break;
                default:
                    throw new Exception("Unexpected property was found to validate.");
            }

            return result;
        }

        private string ValidateQuantity()
        {
            string result = null;

            if (this.Quantity < 0)
            {
                result = "Quantity must be greater than 0.";
            }

            return result;
        }
    }
}