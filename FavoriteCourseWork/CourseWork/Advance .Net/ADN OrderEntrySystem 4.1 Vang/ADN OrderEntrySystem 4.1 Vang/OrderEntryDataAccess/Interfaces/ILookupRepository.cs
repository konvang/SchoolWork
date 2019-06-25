using OrderEntryEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntryDataAccess
{
    public interface ILookupRepository : IRepository
    {
        IEnumerable<ILookupEntity> LookupList { get; }
    }
}