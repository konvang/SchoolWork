using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntryEngine
{
    public class EntityEventArgs<T>
    {
        public EntityEventArgs(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}