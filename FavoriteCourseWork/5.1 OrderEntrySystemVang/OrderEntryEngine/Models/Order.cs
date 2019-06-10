using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderEntryEngine
{
    public class Order
    {
        private decimal shippingAmount;

        private decimal productTotal;

        private decimal taxTotal;

        public Order()
        {
            this.Lines = new List<OrderLine>();
        }

        public int Id { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<OrderLine> Lines { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsArchived { get; set; }

        public decimal ShippingAmount
        {
            get
            {
                return this.shippingAmount;
            }
            set
            {
                this.shippingAmount = Math.Round(value, 2);
            }
        }

        public decimal ProductTotal
        {
            get
            {
                return this.productTotal;
            }
            set
            {
                this.productTotal = Math.Round(value, 2);
            }
        }

        public decimal TaxTotal
        {
            get
            {
                return this.taxTotal;
            }
            set
            {
                this.taxTotal = Math.Round(value, 2);
            }
        }

        public decimal Total
        {
            get
            {
                return this.ProductTotal + this.TaxTotal + this.ShippingAmount;
            }
        }

        public void CalculateTotals()
        {
            this.ProductTotal = this.Lines.Sum(l => l.ExtendedProductAmount);
            this.TaxTotal = this.Lines.Sum(l => l.ExtendedTax);
        }

        public void Post()
        {
            switch (this.Status)
            {
                case OrderStatus.Pending:
                    this.Status = OrderStatus.Placed;

                    foreach (OrderLine line in this.Lines)
                    {
                        line.Post();
                        line.CalculateTax();
                    }

                    this.CalculateTotals();

                    break;
                case OrderStatus.Placed:

                    break;
                case OrderStatus.Shipped:

                    break;
                default:
                    break;
            }
        }
    }
}