using Core.Common;
using Core.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBoilerPlate.Data
{
    /// <summary>
    /// Importable class that represents a factory of Repositories
    /// </summary>
    public class DataRepositoryFactory : IDataRepositoryFactory
    {
        private readonly IServiceProvider services;

        public DataRepositoryFactory() { }

        public DataRepositoryFactory(IServiceProvider services)
        {
            this.services = services;
        }

        public IDataRepository<TEntity> GetDataRepository<TEntity>() where TEntity : class, new()
        {
            //Import instance of T from the DI container
            var instance = services.GetService<IDataRepository<TEntity>>();

            return instance;
        }

        public TRepository GetCustomDataRepository<TRepository>() where TRepository : IDataRepository
        {
            //Import instance of the repository from the DI container
            var instance = services.GetService<TRepository>();

            return instance;
        }

        public IUnitOfWork GetUnitOfWork()
        {
            //Import instance of T from the DI container
            var instance = services.GetService<IUnitOfWork>();

            return instance;
        }

        public IUnitOfWork GetUnitOfWork<T>() where T : IUnitOfWork
        {
            //Import instance of T from the DI container
            var instance = services.GetService<T>();

            return instance;
        }
    }
}
