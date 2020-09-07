using Core.Common.Contracts;
using Core.Common.Exceptions;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Web.Infrastructure.Settings;
using MyBoilerPlate.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyBoilerPlate.Web.Infrastructure
{
    public class UserProfile : IUserProfile
    {
        private readonly IEnumerable<Claim> _Claims;
        private readonly IServiceProvider _ServiceLocator;
        private readonly AppSettings _AppSettings;

        public Guid UserId
        {
            get
            {
                if(_Claims.Any(x => x.Type == "sub"))
                {
                    return Guid.Parse(_Claims.First(x => x.Type == "sub").Value);
                }

                return Guid.Empty;
            }
        }

        public Guid RoleId
        {
            get
            {
                if(_Claims.Any(x => x.Type == "roleid"))
                {
                    return Guid.Parse(_Claims.First(x => x.Type == "roleid").Value);
                }

                return Guid.Empty;
            }
        }

        public string UserName
        {
            get
            {
                if(_Claims.Any(x => x.Type == "name"))
                {
                    return _Claims.First(x => x.Type == "name").Value;
                }

                return null;
            }
        }

        public string FullName
        {
            get
            {
                if(_Claims.Any(x => x.Type == ClaimTypes.GivenName))
                {
                    return _Claims.First(x => x.Type == ClaimTypes.GivenName).Value;
                }

                return null;
            }
        }

        public string RoleName
        {
            get
            {
                if(_Claims.Any(x => x.Type == ClaimTypes.Role))
                {
                    return _Claims.First(x => x.Type == ClaimTypes.Role).Value;
                }

                return null;
            }
        }

        public string Email
        {
            get
            {
                if(_Claims.Any(x => x.Type == ClaimTypes.Email))
                {
                    return _Claims.First(x => x.Type == ClaimTypes.Email).Value;
                }

                return null;
            }
        }

        public UserProfile(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceLocator, AppSettings appSettings)
        {
            IHttpContextAccessor context;
            if(httpContextAccessor.HttpContext.RequestServices != null)
            {
                context = (IHttpContextAccessor)httpContextAccessor.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));
            }
            else  //Test Mode
            {
                context = new HttpContextAccessor()
                {
                    HttpContext = httpContextAccessor.HttpContext
                };
            }

            this._Claims = context.HttpContext.User.Claims;
            this._ServiceLocator = serviceLocator;
            this._AppSettings = appSettings;
        }

        public bool HasPermission(string permissionName)
        {
            return GetAuthorizationEngine().GetUserAndRolePermissions(_AppSettings.ApplicationId, this.UserId).Result.Any(x => x == permissionName);
        }

        private IAuthorizationEngine _AuthorizationEngine;

        private IAuthorizationEngine GetAuthorizationEngine()
        {
            ////Sample
            //if(_AuthorizationEngine == null)
            //    _AuthorizationEngine = (IAuthorizationEngine)this._ServiceLocator.GetService(typeof(IAuthorizationEngine));

            //return _AuthorizationEngine;


            //Retrieve the engine that validate the permissions
            throw new NotImplementedException();
        }
    }
}
