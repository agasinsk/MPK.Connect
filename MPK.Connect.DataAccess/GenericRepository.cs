using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MPK.Connect.DataAccess
{
    public abstract class GenericRepository<TContext, T> :
        IGenericRepository<T> where T : class where TContext : DbContext, new()
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
            var query = Context.Set<T>().Where(predicate);
            return query;
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = Context.Set<T>();
            return query;
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }
    }
}