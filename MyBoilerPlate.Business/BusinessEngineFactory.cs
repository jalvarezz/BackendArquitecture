using Core.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyBoilerPlate.Business
{
    /// <summary>
    /// Importable class that represents a factory of Business Engines
    /// </summary>
    public class BusinessEngineFactory : IBusinessEngineFactory
    {
        private readonly IServiceProvider _Services;

        public BusinessEngineFactory(IServiceProvider services)
        {
            this._Services = services;
        }

        public T GetBusinessEngine<T>() where T : IBusinessEngine
        {
            //Import instance of T from the DI container
            return _Services.GetService<T>();
        }
    }
}
