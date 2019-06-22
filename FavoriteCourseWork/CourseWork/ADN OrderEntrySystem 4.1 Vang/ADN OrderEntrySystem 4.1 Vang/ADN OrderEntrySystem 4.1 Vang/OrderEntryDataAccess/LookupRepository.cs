using OrderEntryEngine;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntryDataAccess
{
    public class LookupRepository<T> : Repository<T>, ILookupRepository where T : class, ILookupEntity
    {
        public LookupRepository(DbSet<T> dbSet)
            : base(dbSet)
        {

        }

        public IEnumerable<ILookupEntity> LookupList
        {
            get
            {
                return GetEntities();
            }
        }
    }
}
