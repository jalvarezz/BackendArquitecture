using Core.Common.Contracts;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Engines.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyBoilerPlate.Business.Entities.DTOs;
using AutoMapper;

namespace MyBoilerPlate.Business.Engines
{
    public class AuthorizationEngine : IAuthorizationEngine
    {
        private readonly ICacheService _CacheService;
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        private readonly IMapper _Mapper;
        private readonly IUserProfile _UserProfile;

        public AuthorizationEngine(ICacheService cacheService,
                                   IDataRepositoryFactory dataRepositoryFactory,
                                   IMapper mapper,
                                   IUserProfile userProfile)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _Mapper = mapper;
            _UserProfile = userProfile;
            _CacheService = cacheService;
        }

        public async Task<string[]> GetUserPermissionsAsync(Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<string[]> GetRolePermissionsAsync(Guid? roleId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<string[]> GetUserAndRolePermissionsAsync(Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfileInformationDTO> GetProfileWithPermissionsAsync(Guid? userId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MenuNodeDTO>> GetSitemapAsync(Guid? userId = null)
        {
            throw new NotImplementedException();
        }
    }
}
