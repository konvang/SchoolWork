using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OrderEntryEngine
{
    public class Location : IDataErrorInfo
    {
        /// <summary>
        /// The property names to validate.
        /// </summary>
        private static readonly string[] propertiesToValidate =
        {
            "Name",
            "Description",
            "City",
            "State"
        };

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(2)]
        public string State { get; set; }

        public bool IsArchived { get; set; }

        public virtual ICollection<Product> Products { get; set; }

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

                foreach (string s in Location.propertiesToValidate)
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

        public override string ToString()
        {
            return this.Name;
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
                case "City":
                    result = this.ValidateCity();
                    break;
                case "State":
                    result = this.ValidateState();
                    break;
                default:
                    throw new Exception("Unexpected property was found to validate.");
            }

            return result;
        }

        private string ValidateState()
        {
            string result = null;

            if (string.IsNullOrWhiteSpace(this.State))
            {
                result = "State is required.";
            }
            else if (!Regex.IsMatch(this.State, @"[A-Z]{2}"))
            {
                result = "State is invalid. Ex: WI";
            }
            else if (this.State != null && this.State.Length != 2)
            {
                result = "State must be two characters.";
            }

            return result;
        }

        private string ValidateCity()
        {
            string result = null;

            if (string.IsNullOrWhiteSpace(this.City))
            {
                result = "City is required.";
            }
            else if (this.City.Length > 100)
            {
                result = "City must be fewer than 100 characters.";
            }

            return result;
        }

        private string ValidateDescription()
        {
            string result = null;

            if (this.Description != null && this.Description.Length > 500)
            {
                result = "Description must be fewer than 500 characters.";
            }

            return result;
        }

        private string ValidateName()
        {
            string result = null;

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                result = "Name is required.";
            }
            else if (this.Name.Length > 100)
            {
                result = "Name must be fewer than 100 characters.";
            }

            return result;
        }
    }
}