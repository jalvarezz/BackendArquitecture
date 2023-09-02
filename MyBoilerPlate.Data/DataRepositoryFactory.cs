using Core.Common.Base;
using Core.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyBoilerPlate.Data
{
    /// <summary>
    /// Importable class that represents a factory of Repositories
    /// </summary>
    public class DataRepositoryFactory : IDataRepositoryFactory
    {
        private readonly IServiceProvider _Services;

        public DataRepositoryFactory() { }

        public DataRepositoryFactory(IServiceProvider services)
        {
            this._Services = services;
        }

        public IDataRepository<TEntity> GetDataRepository<TEntity>() where TEntity : class, new()
        {
            //Import instance of T from the DI container
            var instance = _Services.GetService<IDataRepository<TEntity>>();

            return instance;
        }

        public IDataRepository<TEntity> GetOracleDataRepository<TEntity>() where TEntity : OracleEntityBase<TEntity>, new()
        {
            //Import instance of T from the DI container
            var instance = _Services.GetService<IOracleDataRepository<TEntity>>();

            return instance;
        }

        public TRepository GetCustomDataRepository<TRepository>() where TRepository : IDataRepository
        {
            //Import instance of the repository from the DI container
            var instance = _Services.GetService<TRepository>();

            return instance;
        }

        public IUnitOfWork GetUnitOfWork()
        {
            //Import instance of T from the DI container
            var instance = _Services.GetService<IUnitOfWork>();

            return instance;
        }

        public IUnitOfWork GetUnitOfWork<T>() where T : IUnitOfWork
        {
            //Import instance of T from the DI container
            var instance = _Services.GetService<T>();

            return instance;
        }
    }
}
