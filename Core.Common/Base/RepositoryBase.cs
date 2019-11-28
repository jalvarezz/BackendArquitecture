using Core.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Core.Common.Extensions;
using Core.Common;
using Microsoft.Extensions.Options;

namespace Core.Common.Base
{
    public abstract class RepositoryBase<TEntity, U> : IDataRepository<TEntity>
        where TEntity : class, new()
        where U : DbContext
    {
        protected readonly U _Context;
        private readonly DbSet<TEntity> _DbSet;

        public RepositoryBase(U context)
        {
            _Context = context;
            _DbSet = _Context.Set<TEntity>();
        }

        public virtual TEntity Add(TEntity entity)
        {
            _Context.Set<TEntity>().Add(entity);

            _Context.SaveChanges();

            return entity;
        }

        public virtual void Remove(TEntity entity)
        {
            _DbSet.Attach(entity);
            _Context.Entry<TEntity>(entity).State = EntityState.Deleted;

            _Context.SaveChanges();
        }

        public virtual void Remove(long id)
        {
            TEntity entity = _DbSet.Find(id);

            _Context.Entry<TEntity>(entity).State = EntityState.Deleted;
            _Context.SaveChanges();
        }

        public void Remove(IEnumerable<TEntity> entities)
        {
            foreach (TEntity current in entities)
            {
                _DbSet.Attach(current);
                _Context.Entry<TEntity>(current).State = EntityState.Deleted;

            }
            _Context.SaveChanges();
        }

        public virtual TEntity Update(TEntity entity)
        {
            _DbSet.Attach(entity);
            _Context.Entry<TEntity>(entity).State = EntityState.Modified;

            _Context.SaveChanges();

            return entity;
        }

        public IEnumerable<TEntity> GetAll(string sortExpression = null)
        {
            return _DbSet.AsNoTracking().OrderBy(sortExpression).ToList();
        }

        public IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null, string sortExpression = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var notSortedResults = transform(query);

            var sortedResults = sortExpression == null ? notSortedResults : notSortedResults.OrderBy(sortExpression);
            
            return sortedResults.ToArray().ToList();
        }
        
        public IEnumerable<TResult> GetAll<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null, string sortExpression = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var notSortedResults = transform(query);

            var sortedResults = sortExpression == null ? notSortedResults : notSortedResults.OrderBy(sortExpression);
          
            return sortedResults.ToArray().ToList();
        }

        public int GetCount<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            return transform(query).Count();
        }

        public IPagedList<TEntity> GetPaged(int startRowIndex, int pageSize, string sortExpression = null)
        {
            return new PagedList<TEntity>(_DbSet.AsNoTracking().OrderBy(sortExpression), startRowIndex, pageSize);
        }

        public IPagedList<TEntity> GetPaged(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null, int startRowIndex = -1, int pageSize = -1, string sortExpression = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var notSortedResults = transform(query);

            var sortedResults = sortExpression == null ? notSortedResults : notSortedResults.OrderBy(sortExpression);

            return new PagedList<TEntity>(sortedResults, startRowIndex, pageSize);
        }

        public IPagedList<TResult> GetPaged<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null, int startRowIndex = -1, int pageSize = -1, string sortExpression = null)
        {
            var query =filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var notSortedResults = transform(query);

            var sortedResults = sortExpression == null ? notSortedResults : notSortedResults.OrderBy(sortExpression);

            return new PagedList<TResult>(sortedResults, startRowIndex, pageSize);
        }

        public TEntity Get(long id)
        {
            return _DbSet.Find(id);
        }

        public TEntity Get(Guid id)
        {
            return _DbSet.Find(id);
        }

        public TResult Get<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null, string sortExpression = null)
        {
            var query = filter == null ? _DbSet : _DbSet.Where(filter);

            var notSortedResults = transform(query);

            var sortedResults = sortExpression == null ? notSortedResults : notSortedResults.OrderBy(sortExpression);

            return sortedResults.FirstOrDefault();
        }
        public bool Exists(long id)
        {
            return _DbSet.Find(id) != null;
        }

        public bool Exists(Func<IQueryable<TEntity>, IQueryable<TEntity>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var result = selector(query);

            return result.Any();
        }

        public bool Exists<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

            var result = selector(query);

            return result.Any();
        }
    }
}
