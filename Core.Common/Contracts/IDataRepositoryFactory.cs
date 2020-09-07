namespace Core.Common.Contracts
{
    public interface IDataRepositoryFactory
    {
        IDataRepository<TEntity> GetDataRepository<TEntity>() where TEntity : class, new();
        IContigencyDataRepository<TEntity> GetContigencyDataRepository<TEntity>() where TEntity : class, new();
        TRepository GetCustomDataRepository<TRepository>() where TRepository : IDataRepository;
        IUnitOfWork GetUnitOfWork();
        IUnitOfWork GetUnitOfWork<T>() where T : IUnitOfWork;
    }
}
