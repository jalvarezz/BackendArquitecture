using Core.Common.Contracts;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Engines.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyBoilerPlate.Business.Entities.DTOs;

namespace MyBoilerPlate.Business.Engines
{
    public class AuthorizationEngine : IAuthorizationEngine
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        private readonly IBusinessEngineFactory _BusinessEngineFactory;
        private readonly IMessageHandler _MessageHandler;

        public AuthorizationEngine(IDataRepositoryFactory dataRepositoryFactory,
                                     IBusinessEngineFactory businessEngineFactory,
                                     IMessageHandler messageHandler)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
            _MessageHandler = messageHandler;
        }

        public Task<string[]> GetRolePermissions(Guid applicationId, Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MenuNodeDTO>> GetSitemap(Guid applicationId, Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetUserAndRolePermissions(Guid applicationId, Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetUserPermissions(Guid applicationId, Guid? userId = null)
        {
            throw new NotImplementedException();
        }
    }
}
