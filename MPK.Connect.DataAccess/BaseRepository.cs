using Microsoft.EntityFrameworkCore;
using MPK.Connect.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MPK.Connect.DataAccess
{
    public abstract class GenericRepository<TContext, T, TId> :
        IGenericRepository<T> where T : IdentifiableEntity<TId> where TId : class where TContext : DbContext, new()
    {
        public TContext Context { get; set; } = new TContext();

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void AddRange(List<T> entities)
        {
            Context.Set<T>().AddRange(entities);
        }

        public virtual T Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            return entity;
        }

        public virtual void Edit(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate); ;
        }

        public virtual IQueryable<T> GetAll()
        {
            return Context.Set<T>(); ;
        }

        public virtual T GetSingle(TId id)
        {
            return Context.Set<T>().FirstOrDefault(t => t.Id == id);
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }
    }
}