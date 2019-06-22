using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OrderEntryEngine;

namespace OrderEntryDataAccess
{
    public class Repository<T>: IRepository where T: class, IEntity
    {
          private DbSet<T> dbSet;

          public Repository(DbSet<T> dbSet)
          {
              this.dbSet = dbSet;
          }

          public event EventHandler<EntityEventArgs<T>> EntityAdded;

          public event EventHandler<EntityEventArgs<T>> EntityRemoved;

          public void AddEntity(T entity)
          {
              if (!this.ContainsEntity(entity))
              {
                  this.dbSet.Add(entity);

                  if (this.EntityAdded != null)
                  {
                      this.EntityAdded(this, new EntityEventArgs<T>(entity));
                  }
              }
          }

         public bool ContainsEntity(T entity)
         {
             return this.GetEntity(entity.Id) != null;
         }
     
         public T GetEntity(int id)
         {
             return this.dbSet.Find(id);
         }
         
         public List<T> GetEntities()
         {
             return this.dbSet.Where(p => !p.IsArchived).ToList();
         }


         public void RemoveEntity(T entity)
         {
             if (entity == null)
             {
                 throw new ArgumentNullException("Entity");
             }

             entity.IsArchived = true;

             if (this.EntityRemoved != null)
             {
                 this.EntityRemoved(this, new EntityEventArgs<T>(entity));
             }
         }

        public void SaveToDatabase()
        {
                RepositoryManager.Context.SaveChanges();
        }
    }
}