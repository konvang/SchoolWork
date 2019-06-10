using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntryEngine
{
    public class ProductEventArgs
    {
        public ProductEventArgs(Product product)
        {
            this.Product = product;
        }

        public Product Product { get; private set; }
    }
}