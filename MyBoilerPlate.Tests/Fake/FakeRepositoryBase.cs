using Core.Common;
using Core.Common.Base;
using Core.Common.Contracts;
using MyBoilerPlate.Data;
using MyBoilerPlate.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBoilerPlate.Tests.Fake
{
    public class FakeRepositoryBase<TEntity> : Repository<TEntity> where TEntity : class, new()
    {
        protected IList<TEntity> FakeData;

        public FakeRepositoryBase(SampleDataContext context) : base(context) {
            FakeData = new List<TEntity>();
        }

        public override async Task<TEntity> AddAsync(TEntity entity)
        {
            return await Task.Run(() =>
            {
                FakeData.Add(entity);

                this.ResolveIdentityProperties(entity);

                return entity;
            });
        }
        
        public override async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entityList)
        {
            return await Task.Run(() =>
            {
                foreach (var item in entityList)
                {
                    FakeData.Add(item);

                    this.ResolveIdentityProperties(item);
                }

                return FakeData;
            });
        }


        public override async Task RemoveAsync(TEntity entity)
        {
            await Task.Run(() => FakeData.Remove(entity));
        }

        public override async Task RemoveAllAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() =>
            {
                foreach (var item in entities)
                {
                    if (FakeData.Any(x => x == item))
                    {
                        FakeData.Remove(item);
                    }
                }
            });
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Task.Run(() =>
            {
                var existingRecord = FakeData.Where(x => x == entity).FirstOrDefault();

                if (existingRecord != null)
                {
                    existingRecord = entity;
                }

                return existingRecord;
            });
        }

        public override async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Task.Run(() => FakeData);
        }

        public override async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Task.Run(() => filter == null ? FakeData : FakeData.Where(filter.Compile()));
        }

        public override async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            return await Task.Run(() =>
            {
                var query = FakeData.AsQueryable();

                var data = filter == null ? query : query.Where(filter);

                var notSortedResults = transform(data);

                return notSortedResults.ToList();
            });
        }

        public override async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            return await Task.Run(() =>
            {
                var query = FakeData.AsQueryable();

                var data = filter == null ? query : query.Where(filter);

                var notSortedResults = transform(query);

                return notSortedResults.ToList();
            });
        }

        public override async Task<IPagedList<TEntity>> GetPagedAsync(int pageIndex, int pageSize)
        {
            return await Task.Run(() => new PagedList<TEntity>(FakeData, pageIndex, pageSize));
        }

        public override async Task<IPagedList<TEntity>> GetPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null, int pageIndex = -1, int pageSize = -1)
        {
            return await Task.Run(() =>
            {
                var query = FakeData.AsQueryable();

                var data = filter == null ? query : query.Where(filter);

                var notSortedResults = transform(data);

                return new PagedList<TEntity>(notSortedResults, pageIndex, pageSize);
            });
        }

        public override async Task<IPagedList<TResult>> GetPagedAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null, int pageIndex = -1, int pageSize = -1)
        {
            return await Task.Run(() =>
            {
                var query = FakeData.AsQueryable();

                var data = filter == null ? query : query.Where(filter);

                var notSortedResults = transform(data);

                return new PagedList<TResult>(notSortedResults, pageIndex, pageSize);
            });
        }

        public override async Task<int> GetCountAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            return await Task.Run(() =>
            {
                var query = FakeData.AsQueryable();

                var data = filter == null ? query : query.Where(filter);

                return transform(query).Count();
            });
        }

        public override async Task<TEntity> GetSingleAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            return await Task.Run(() =>
            {
                var query = FakeData.AsQueryable();

                var data = filter == null ? query : query.Where(filter);

                var notSortedResults = transform(data);

                return notSortedResults.FirstOrDefault();
            });
        }

        public override async Task<TResult> GetSingleAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            return await Task.Run(() =>
            {
                var query = FakeData.AsQueryable();

                var data = filter == null ? query : query.Where(filter);

                var notSortedResults = transform(data);

                return notSortedResults.FirstOrDefault();
            });
        }

        public override async Task<bool> ExistsAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            return await Task.Run(() =>
            {
                var query = FakeData.AsQueryable();

                var data = filter == null ? query : query.Where(filter);

                var notSortedResults = transform(data);

                return notSortedResults.Any();
            });
        }

        public override async Task<bool> ExistsAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
        {
            return await Task.Run(() =>
            {
                var query = FakeData.AsQueryable();

                var data = filter == null ? query : query.Where(filter);

                var notSortedResults = transform(data);

                return notSortedResults.Any();
            });
        }

        private TEntity ResolveIdentityProperties(TEntity entity)
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            foreach(PropertyInfo property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(DatabaseGeneratedAttribute)) as DatabaseGeneratedAttribute;

                if(attribute != null) // This property has a DatabaseGeneratedAttribute
                {
                    if(attribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                    {
                        var propTypeName = property.PropertyType.Name.ToLower();

                        switch(propTypeName)
                        {
                            case "int32":
                            case "int64":
                            case "decimal":
                                Random r = new Random();
                                int rInt = r.Next(100000000, 200000000);

                                property.SetValue(entity, rInt, null);
                                break;
                            case "guid":
                                property.SetValue(entity, Guid.NewGuid(), null);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return entity;
        }
    }
}
