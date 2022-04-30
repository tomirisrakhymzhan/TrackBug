using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TrackBug.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);


        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        bool Any(Expression<Func<T, bool>> filter);
        IEnumerable<T> Except(IEnumerable<T> secondSet);
    }
}
