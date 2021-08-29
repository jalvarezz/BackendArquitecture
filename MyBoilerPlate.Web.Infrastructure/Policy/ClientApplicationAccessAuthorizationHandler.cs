using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using Serilog;
using MyBoilerPlate.Web.Models;
using System.Net.Mime;
using MyBoilerPlate.Business.Entities;

namespace MyBoilerPlate.Web.Infrastructure.Policy
{
    public class ClientApplicationAccessAuthorizationHandler : AuthorizationHandler<ClientApplicationAccessRequirement>
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        private readonly ICacheService _CacheService;
        private readonly IApplicationProfile _ApplicationProfile;

        public ClientApplicationAccessAuthorizationHandler(IHttpContextAccessor httpContextAccessor, 
                                                           IDataRepositoryFactory dataRepositoryFactory,
                                                           ICacheService cacheService,
                                                           IApplicationProfile applicationProfile)
        {
            _HttpContextAccessor = httpContextAccessor;
            _DataRepositoryFactory = dataRepositoryFactory;
            _CacheService = cacheService;
            _ApplicationProfile = applicationProfile;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientApplicationAccessRequirement requirement)
        {
            var defaultMessage = "No Autorizado";

            var authHeader = _HttpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];

            if (!string.IsNullOrEmpty(authHeader) && authHeader.ToString().StartsWith("Basic"))
            {
                string accessKey = authHeader.ToString().Substring("Basic ".Length).Trim();

                _ApplicationProfile.ApplicationId = await _CacheService.GetCachedAsync(accessKey, async () =>
                {
                    var repo = _DataRepositoryFactory.GetDataRepository<ApplicationKey>();

                    var result = await repo.GetAsync(x => x.Select(r => r.ApplicationId), x => !x.IsDeleted && x.AccessKey == accessKey);

                    return result;
                }, TimeSpan.FromMinutes(5));

                if (_ApplicationProfile.ApplicationId != null)
                {
                    context.Succeed(requirement);
                    return;
                }

                defaultMessage = "The provided Access Key is invalid.";
            }

            var responseModel = new ApiErrorResponseModel()
            {
                Message = defaultMessage
            };

            var result = JsonConvert.SerializeObject(responseModel);
            _HttpContextAccessor.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
            _HttpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await _HttpContextAccessor.HttpContext.Response.WriteAsync(result);
        }
    }
}
