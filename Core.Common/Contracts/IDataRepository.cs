using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IDataRepository
    {
    }

    public interface IDataRepository<T> : IDataRepository
        where T : class, new()
    {
        ValueTask<T> AddAsync(T entity);

        ValueTask<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entityList);

        ValueTask RemoveAllAsync(IEnumerable<T> entities);

        ValueTask RemoveAsync(T entity);

        ValueTask SoftRemoveAllAsync(IEnumerable<IDeleteableEntity> entities);

        ValueTask SoftRemoveAsync(IDeleteableEntity entity);

        ValueTask<T> UpdateAsync(T entity);

        ValueTask<TResult> GetAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null);

        ValueTask<IEnumerable<T>> GetAllAsync();

        ValueTask<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        ValueTask<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> transform, Expression<Func<T, bool>> filter = null);

        ValueTask<IEnumerable<TResult>> GetAllAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null);

        ValueTask<IPagedList<T>> GetPagedAsync(int pageIndex, int pageSize);

        ValueTask<IPagedList<T>> GetPagedAsync(Func<IQueryable<T>, IQueryable<T>> transform, Expression<Func<T, bool>> filter = null, int pageIndex = -1, int pageSize = -1);

        ValueTask<IPagedList<TResult>> GetPagedAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null, int pageIndex = -1, int pageSize = -1);

        ValueTask<int> GetCountAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null);

        ValueTask<bool> ExistsAsync(Expression<Func<T, bool>> filter = null);

        ValueTask<bool> ExistsAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null);

        ValueTask<bool> ExistsAsync(Func<IQueryable<T>, IQueryable<T>> transform, Expression<Func<T, bool>> filter = null);
    }
}
