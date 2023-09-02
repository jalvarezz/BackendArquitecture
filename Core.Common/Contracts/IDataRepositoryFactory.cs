using Core.Common.Base;

namespace Core.Common.Contracts
{
    public interface IDataRepositoryFactory
    {
        IDataRepository<TEntity> GetDataRepository<TEntity>() where TEntity : class, new();
        IDataRepository<TEntity> GetOracleDataRepository<TEntity>() where TEntity : OracleEntityBase<TEntity>, new();
        TRepository GetCustomDataRepository<TRepository>() where TRepository : IDataRepository;
        IUnitOfWork GetUnitOfWork();
        IUnitOfWork GetUnitOfWork<T>() where T : IUnitOfWork;
    }
}
