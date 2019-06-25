using OrderEntryEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntrySystem
{
   public class EntityControlAttribute : Attribute
    {
        public EntityControlAttribute(ControlType controlType, int sequence)
        {
            this.ControlType = controlType;
        }

        public EntityControlAttribute(ControlType control, String description, int sequence)
        {
            this.Description = description;
            this.ControlType = control;
            this.Sequence = sequence;
        }

        public ControlType ControlType { get; set; }

        public String Description { get; set; }

        public int Sequence { get; set; }
    }
}
