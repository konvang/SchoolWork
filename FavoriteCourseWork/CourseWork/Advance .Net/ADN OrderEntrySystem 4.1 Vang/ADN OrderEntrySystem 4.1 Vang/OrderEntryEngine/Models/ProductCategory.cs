﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntryEngine
{
    public class ProductCategory : IEntity
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public virtual Game Product { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public bool IsArchived { get; set; }
    }
}