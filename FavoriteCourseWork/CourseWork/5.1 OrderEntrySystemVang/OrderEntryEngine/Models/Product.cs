using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OrderEntryEngine
{
    public class Product : IDataErrorInfo
    {
        /// <summary>
        /// The property names to validate.
        /// </summary>
        private static readonly string[] propertiesToValidate =
        {
            "Name",
            "Description",
            "Price",
            "Quantity"
        };

        public Product()
        {
            this.ProductCategories = new List<ProductCategory>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public Condition Condition { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

        public virtual ICollection<OrderLine> Orders { get; set; }

        public bool IsArchived { get; set; }

        public int Quantity { get; set; }

        public decimal Cost { get; set; }

        public override string ToString()
        {
            return this.Name;
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

                foreach (string s in Product.propertiesToValidate)
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
                case "Name":
                    result = this.ValidateName();
                    break;
                case "Description":
                    result = this.ValidateDescription();
                    break;
                case "Price":
                    result = this.ValidatePrice();
                    break;
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

        private string ValidatePrice()
        {
            string result = null;

            if (this.Price < 0)
            {
                result = "Price must be greater than 0.";
            }

            return result;
        }

        private string ValidateDescription()
        {
            string result = null;

            if (this.Description != null && this.Description.Length > 500)
            {
                result = "Description must be 500 characters or fewer.";
            }

            return result;
        }

        private string ValidateName()
        {
            string result = null;

            if (string.IsNullOrEmpty(this.Name))
            {
                result = "Name is required.";
            }
            else if (!Regex.IsMatch(this.Name, @"([A-Za-z0-9-&!() ])+"))
            {
                result = "Name can contain only letters, numbers and the following special characters: -&!()";
            }
            else if (this.Name.Length > 100)
            {
                result = "Name must be 100 characters or fewer.";
            }

            return result;
        }
    }
}