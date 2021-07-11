using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Business
{
    /// <summary>
    /// Importable class that represents a factory of Email Gateways
    /// </summary>
    public class GatewayFactory : IGatewayFactory
    {
        private readonly IServiceProvider services;

        public GatewayFactory(IServiceProvider services)
        {
            this.services = services;
        }

        public T GetGateway<T>(string name) where T : IGateway
        {
            //Get referenced assemblies
            var interfaceType = AppDomain.CurrentDomain.GetAssemblies()
                                                       .Where(x => x.FullName.Contains("MyBoilerPlate.Gateways"))
                                                       .SelectMany(x => x.ExportedTypes)
                                                       .Where(x => typeof(T).IsAssignableFrom(x) &&
                                                                   x.IsInterface &&
                                                                   x.Name == $"I{name}Gateway")
                                                       .FirstOrDefault();

            if (interfaceType == null)
                throw new InvalidOperationException($"Gateway '{name}' is not implemented");

            //Import instance from the DI container
            var gateway = (T)services.GetService(interfaceType);

            return gateway;
        }
    }
}
