using Core.Common.Base;

namespace MyBoilerPlate.Data
{
    public class Repository<TEntity> : RepositoryBase<TEntity, SampleDataContext> where TEntity : class, new()
    {
        public Repository(SampleDataContext context) : base(context) {  }
    }
}
