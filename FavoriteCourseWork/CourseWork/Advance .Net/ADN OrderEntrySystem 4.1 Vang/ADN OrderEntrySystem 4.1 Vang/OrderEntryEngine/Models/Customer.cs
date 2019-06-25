using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OrderEntryEngine
{
    public class Customer : IDataErrorInfo, IEntity, ILookupEntity
    {
        /// <summary>
        /// The property names to validate.
        /// </summary>
        private static readonly string[] propertiesToValidate =
        {
            "FirstName",
            "LastName",
            "Phone",
            "Email",
            "Address",
            "City",
            "State"
        };

        public Customer()
        {
            this.Orders = new List<Order>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(150)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(2)]
        public string State { get; set; }

        public bool IsArchived { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public string Name
        {
            get
            {
                return string.Format("{0}, {1}", this.LastName, this.FirstName);
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

                foreach (string s in Customer.propertiesToValidate)
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
            return this.FirstName + " " + this.LastName;
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
                case "FirstName":
                    result = this.ValidateFirstName();
                    break;
                case "LastName":
                    result = this.ValidateLastName();
                    break;
                case "Phone":
                    result = this.ValidatePhone();
                    break;
                case "Email":
                    result = this.ValidateEmail();
                    break;
                case "Address":
                    result = this.ValidateAddress();
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

            if (this.State != null && this.State.Length != 2)
            {
                result = "State must be two characters.";
            }
            else if (this.State != null && !Regex.IsMatch(this.State, @"[A-Z]{2}"))
            {
                result = "State is invalid. Ex: WI";
            }

            return result;
        }

        private string ValidateCity()
        {
            string result = null;

            if (this.City != null && this.City.Length > 100)
            {
                result = "City must be fewer than 100 characters.";
            }

            return result;
        }

        private string ValidateAddress()
        {
            string result = null;

            if (this.Address != null && this.Address.Length > 150)
            {
                result = "Address must be fewer than 150 characters.";
            }

            return result;
        }

        private string ValidateEmail()
        {
            string result = null;

            if (this.Email != null && this.Email.Length > 255)
            {
                result = "Email must be fewer than 255 characters.";
            }
            else if (this.Email != null && !Regex.IsMatch(this.Email, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                result = "Email is invalid. Ex: smith@example.com";
            }

            return result;
        }

        private string ValidatePhone()
        {
            string result = null;

            if (this.Phone != null && !Regex.IsMatch(this.Phone, @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"))
            {
                result = "Phone is invalid. Ex: 123-456-7890";
            }

            return result;
        }

        private string ValidateLastName()
        {
            string result = null;

            if (string.IsNullOrWhiteSpace(this.LastName))
            {
                result = "Last name is required.";
            }
            else if (this.LastName.Length > 100)
            {
                result = "Last name must be fewer than 100 characters.";
            }

            return result;
        }

        private string ValidateFirstName()
        {
            string result = null;

            if (string.IsNullOrWhiteSpace(this.FirstName))
            {
                result = "First name is required.";
            }
            else if (this.FirstName.Length > 100)
            {
                result = "First name must be fewer than 100 characters.";
            }

            return result;
        }
    }
}