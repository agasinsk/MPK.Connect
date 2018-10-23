using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MPK.Connect.DataAccess
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);

        int AddRange(List<T> entities);

        int BulkInsert(List<T> entities);

        int BulkMerge(List<T> entities);

        bool Contains(T entity);

        T Delete(T entity);

        void Edit(T entity);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

        void Save();
    }
}