using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntryEngine
{
   public interface IEntity
    {
        int Id { get; set; }

        bool IsArchived { get; set; }
    }
}