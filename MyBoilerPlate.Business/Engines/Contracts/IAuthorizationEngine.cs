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
        Task<string[]> GetUserPermissionsAsync(Guid? userId = null);

        Task<string[]> GetRolePermissionsAsync(Guid? roleId = null);

        Task<string[]> GetUserAndRolePermissionsAsync(Guid? userId = null);

        Task<UserProfileInformationDTO> GetProfileWithPermissionsAsync(Guid? userId = null);

        Task<IEnumerable<MenuNodeDTO>> GetSitemapAsync(Guid? userId = null);

        Task<UserProfileDTO> GetUserProfileAsync(Guid? userId = null);
    }
}