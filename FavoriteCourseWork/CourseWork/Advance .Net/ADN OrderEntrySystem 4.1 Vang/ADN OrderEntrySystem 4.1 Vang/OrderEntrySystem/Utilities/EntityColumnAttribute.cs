using System;

namespace OrderEntrySystem
{
    public class EntityColumnAttribute : Attribute
    {
        public EntityColumnAttribute(int sequence, int width)
           : this(string.Empty, sequence, width)
        {

        }

        public EntityColumnAttribute(string description, int sequence, int width)
        {
            this.Description = description;
            this.Sequence = sequence;
            this.Width = width;
        }

        public int Sequence { get; set; }

        public int Width { get; set; }

        public string Description { get; set; }
    }
}