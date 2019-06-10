using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntryEngine
{
    public class Developer : ILookupEntity
    {
        public string Name { get; set; }

        public bool IsArchived { get; set; }

        public int Id { get; set; }
    }
}