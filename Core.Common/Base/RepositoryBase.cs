using Core.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Common.Base
{
    public abstract class RepositoryBase<TEntity, U> : IDataRepository<TEntity>
        where TEntity : class, new()
        where U : DbContext
    {
        protected readonly U _Context;
        private readonly DbSet<TEntity> _DbSet;

        protected RepositoryBase(U context)
        {
            _Context = context;
            _DbSet = _Context.Set<TEntity>();
        }

        public virtual async ValueTask<TEntity> AddAsync(TEntity entity)
        {
            await _Context.Set<TEntity>().AddAsync(entity);

            await _Context.SaveChangesAsync();

            return entity;
        }

        public virtual async ValueTask<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entityList)
        {
            await _Context.Set<TEntity>().AddRangeAsync(entityList);

            await _Context.SaveChangesAsync();

            return entityList;
        }

        public virtual async ValueTask RemoveAsync(TEntity entity)
        {
            _DbSet.Attach(entity);

            _Context.Entry<TEntity>(entity).State = EntityState.Deleted;

            await _Context.SaveChangesAsync();
        }

        public virtual async ValueTask RemoveAllAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity current in entities)
            {
                _DbSet.Attach(current);
                _Context.Entry<TEntity>(current).State = EntityState.Deleted;

            }

            await _Context.SaveChangesAsync();
        }

        public virtual async ValueTask SoftRemoveAsync(IDeleteableEntity entity)
        {
            entity.IsDeleted = true;

            var entityToUpdate = entity as TEntity;

            _DbSet.Attach(entityToUpdate);

            _Context.Entry<TEntity>(entityToUpdate).State = EntityState.Modified;

            await _Context.SaveChangesAsync();
        }

        public virtual async ValueTask SoftRemoveAllAsync(IEnumerable<IDeleteableEntity> entities)
        {
            foreach (IDeleteableEntity current in entities)
            {
                current.IsDeleted = true;

                var entityToUpdate = current as TEntity;

                _DbSet.Attach(entityToUpdate);
                _Context.Entry<TEntity>(entityToUpdate).State = EntityState.Modified;
            }

            await _Context.SaveChangesAsync();
        }

        public virtual async ValueTask<TEntity> UpdateAsync(TEntity entity)
        {
            _DbSet.Attach(entity);
            _Context.Entry<TEntity>(entity).State = EntityState.Modified;

            await _Context.SaveChangesAsync();

            return entity;
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAllAsync()
        {
            var query = _DbSet.AsNoTracking();

            return await query.ToListAsync();
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            var query = _DbSet.AsNoTracking().Where(filter);

            return await query.ToListAsync();
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var notSortedResults = transform(query);

            return await notSortedResults.ToListAsync();
        }

        public virtual async ValueTask<IEnumerable<TResult>> GetAllAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var notSortedResults = transform(query);

            return await notSortedResults.ToListAsync();
        }

        public virtual async ValueTask<IPagedList<TEntity>> GetPagedAsync(int pageIndex, int pageSize)
        {
            return await Task.Run(() =>
            {
                var query = _DbSet.AsNoTracking();

                return new PagedList<TEntity>(query, pageIndex, pageSize);
            });
        }

        public virtual async ValueTask<IPagedList<TEntity>> GetPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null, int pageIndex = -1, int pageSize = -1)
        {
            return await Task.Run(() =>
            {
                var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

                var notSortedResults = transform(query);

                return new PagedList<TEntity>(notSortedResults, pageIndex, pageSize);
            });
        }

        public virtual async ValueTask<IPagedList<TResult>> GetPagedAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null, int pageIndex = -1, int pageSize = -1)
        {
            return await Task.Run(() =>
            {
                var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

                var notSortedResults = transform(query);

                return new PagedList<TResult>(notSortedResults, pageIndex, pageSize);
            });
        }

        public virtual async ValueTask<int> GetCountAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            return await transform(query).CountAsync();
        }

        public virtual async ValueTask<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            TResult result = await Task.Run(async () =>
            {
                var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

                var notSortedResults = transform(query);

                return await notSortedResults.FirstOrDefaultAsync();
            });

            return result;
        }

        public virtual async ValueTask<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var result = await _DbSet.AnyAsync(filter);

            return result;
        }

        public virtual async ValueTask<bool> ExistsAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var result = transform(query);

            return await result.AnyAsync();
        }

        public virtual async ValueTask<bool> ExistsAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var result = transform(query);

            return await result.AnyAsync();
        }
    }
}
