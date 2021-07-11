using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Tests.Fake.Engines
{
    public class AuthorizationFakeEngine : IAuthorizationEngine
    {
        private readonly string[] _Permissions;
        private readonly IServiceProvider _ServiceLocator;

        public AuthorizationFakeEngine(IServiceProvider serviceLocator, Func<string[]> permissionDelegate)
        {
            if (permissionDelegate != null)
                this._Permissions = permissionDelegate();
            else
                this._Permissions = new string[0];

            this._ServiceLocator = serviceLocator;
        }

        public Task<UserProfileInformationDTO> GetProfileWithPermissionsAsync(Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<string[]> GetRolePermissionsAsync(Guid? roleId = null)
        {
            return await Task.Run(() => _Permissions);
        }

        public async Task<IEnumerable<MenuNodeDTO>> GetSitemapAsync(Guid? userId = null)
        {
            // TODO: Update when there is a fake repo and data on the database
            return await Task.Run(() => new List<MenuNodeDTO>());
        }

        public async Task<string[]> GetUserAndRolePermissionsAsync(Guid? userId = null)
        {
            return await Task.Run(() => _Permissions);
        }

        public async Task<string[]> GetUserPermissionsAsync(Guid? userId = null)
        {
            return await Task.Run(() => _Permissions);
        }

        public Task<UserProfileDTO> GetUserProfileAsync(Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasAllowedRoleAsync(Guid? userId)
        {
            throw new NotImplementedException();
        }
    }
}
