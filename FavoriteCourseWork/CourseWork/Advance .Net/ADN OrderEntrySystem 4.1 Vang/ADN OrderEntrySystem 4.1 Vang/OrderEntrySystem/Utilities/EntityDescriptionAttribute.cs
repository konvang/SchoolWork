using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntrySystem
{
    public class EntityDescriptionAttribute : Attribute
    {
        public string Description { get; set; }

        public EntityDescriptionAttribute(string description)
        {
            this.Description = description;
        }
    }
}
