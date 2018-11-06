using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using MPK.Connect.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MPK.Connect.DataAccess
{
    public abstract class GenericRepository<TEntity, TId> :
        IGenericRepository<TEntity> where TEntity : IdentifiableEntity<TId> where TId : class
    {
        protected DbContext Context { get; set; }

        protected GenericRepository(IMpkContext context)
        {
            Context = context as DbContext;
        }

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual int AddRange(List<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
            return entities.Count;
        }

        public int BulkInsert(List<TEntity> entities)
        {
            Context.BulkInsert(entities);
            return entities.Count;
        }

        public bool Contains(TEntity entity)
        {
            return Context.Set<TEntity>().Any(e => e.Id == entity.Id);
        }

        public virtual TEntity Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return entity;
        }

        public virtual void Edit(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate); ;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>(); ;
        }

        public virtual TEntity GetSingle(TId id)
        {
            return Context.Set<TEntity>().FirstOrDefault(t => t.Id == id);
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }
    }
}