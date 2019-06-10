using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntryEngine
{
    public class ProductEventArgs
    {
        public ProductEventArgs(Game product)
        {
            this.Product = product;
        }

        public Game Product { get; private set; }
    }
}