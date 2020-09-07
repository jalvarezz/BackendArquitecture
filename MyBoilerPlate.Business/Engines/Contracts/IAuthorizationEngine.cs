using Core.Common.Contracts;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Business.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBoilerPlate.Business.Engines.Contracts
{
    public interface IAuthorizationEngine : IBusinessEngine
    {
        Task<string[]> GetUserPermissions(Guid applicationId, Guid? userId = null);

        Task<string[]> GetRolePermissions(Guid applicationId, Guid? userId = null);

        Task<string[]> GetUserAndRolePermissions(Guid applicationId, Guid? userId = null);

        Task<IEnumerable<MenuNodeDTO>> GetSitemap(Guid applicationId, Guid? userId = null);
    }
}