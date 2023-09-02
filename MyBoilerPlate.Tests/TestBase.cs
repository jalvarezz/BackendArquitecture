using MyBoilerPlate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Core.Common.Contracts;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using MyBoilerPlate.Web;
using System.Linq;
using MyBoilerPlate.Web.Api;
using System.Collections.Generic;
using MyBoilerPlate.Web.Infrastructure;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Web.Infrastructure.Settings;
using System.ComponentModel.DataAnnotations;
using MyBoilerPlate.Tests.Fake.Engines;
using MyBoilerPlate.Web.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace MyBoilerPlate.Tests
{
    public class TestBase
    {
        private Mock<IHttpContextAccessor> _MockHttpcontext;

        private Func<IList<Claim>> UserClaims { get; set; }
        private Func<string[]> UserPermissions { get; set; }

        protected ServiceProvider ObjectContainer { get; private set; }

        protected void Initialize(Action<ServiceCollection, IConfigurationRoot> addServices, bool installApiControllers = false)
        {
            var services = new ServiceCollection();
            var configuration = TestConfigurationBuilder.GetIConfigurationRoot(AppDomain.CurrentDomain.BaseDirectory);

            addServices(services, configuration);

            if (installApiControllers)
            {
                var controllerBaseType = typeof(ApiControllerBase);

                var controllerTypes = typeof(Startup).Assembly.DefinedTypes.Where(x => x != controllerBaseType &&
                    controllerBaseType.IsAssignableFrom(x)).ToList();

                controllerTypes.ForEach(controllerType =>
                {
                    services.AddTransient(controllerType);
                });
            }

            services.AddOptions();

            //Mock Http for User Profile
            _MockHttpcontext = new Mock<IHttpContextAccessor>();

            IList<Claim> claimCollection = new List<Claim>
                {
                    new Claim("sub", "d9527ab2-94b3-4f4a-a4c3-69fa94eba8e2") //Test User UNIQUE ID
                };

            var identityMock = new Mock<ClaimsIdentity>();
            identityMock.Setup(x => x.Claims).Returns(claimCollection);

            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            cp.Setup(m => m.Identity).Returns(identityMock.Object);

            cp.SetupGet(m => m.Claims).Returns(() =>
            {
                var result = new List<Claim>();

                result.AddRange(claimCollection);

                if (this.UserClaims != null)
                    result.AddRange(this.UserClaims());

                if (result.Count(x => x.Type == "sub") > 1)
                    result.RemoveAt(result.FindIndex(x => x.Type == "sub"));

                return result;
            });

            var hc = new Mock<HttpContext>();
            hc.Setup(h => h.User).Returns(cp.Object);

            _MockHttpcontext.Setup(h => h.HttpContext).Returns(hc.Object);

            services.AddScoped<IUserProfile>(serviceProvider =>
            {
                return new UserProfile(_MockHttpcontext.Object, serviceProvider);
            });

            services.AddScoped<IAuthorizationEngine>(serviceProvider => new AuthorizationFakeEngine(serviceProvider, this.UserPermissions));

            services.AddScoped(serviceProvider => serviceProvider);

            // Build the service provider
            ObjectContainer = services.BuildServiceProvider();
        }

        protected IUserProfile BuildUserProfile(IList<Claim> claims, params string[] permissions)
        {
            //Set UserProfile claims
            if (claims != null && claims.Count > 0)
                UserClaims = () => claims;

            UserPermissions = () => permissions;

            return ObjectContainer.GetService<IUserProfile>();
        }

        protected IUserProfile BuildUserProfile(IList<Claim> claims, List<string> permissions = null)
        {
            //Set UserProfile claims
            if (claims != null && claims.Count > 0)
                this.UserClaims = () => claims;

            if (permissions != null)
                this.UserPermissions = () => permissions.ToArray();

            return ObjectContainer.GetService<IUserProfile>();
        }

        protected void SetUserProfileProperties(IList<Claim> claims, List<string> permissions = null)
        {
            this.UserClaims = () => claims;

            if (permissions != null)
                this.UserPermissions = () => permissions.ToArray();
        }

        public void NoExceptionThrown<T>(Action codeToExecute) where T : Exception
        {
            try
            {
                codeToExecute();
            }
            catch (T ex)
            {
                Debug.WriteLine(ex.Message);

                Assert.Fail("Expected no {0} to be thrown.", typeof(T).Name);
            }
        }

        public void TestRepo<T>() where T : class, new()
        {
            IDataRepository<T> repo = ObjectContainer.GetService<IDataRepository<T>>();

            Debug.WriteLine($"Testing {typeof(T).Name} Repository");

            NoExceptionThrown<Exception>(async () => { await repo.GetAsync(x => x); });
        }

        protected IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        protected bool TryValidateModel(object model)
        {
            return !ValidateModel(model).Any();
        }
    }
}
