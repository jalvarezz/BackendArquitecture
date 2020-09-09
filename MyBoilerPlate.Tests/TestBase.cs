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

namespace MyBoilerPlate.Tests
{
    public class TestBase
    {
        private Mock<IHttpContextAccessor> _MockHttpcontext;

        private Func<IList<Claim>> UserClaims { get; set; }
        private Func<string[]> UserPermissions { get; set; }

        protected ServiceProvider ObjectContainer { get; private set; }

        protected void Initialize(IEnumerable<Type> installerTypes, bool installApiControllers = false)
        {
            var services = new ServiceCollection();
            var configuration = TestConfigurationBuilder.GetIConfigurationRoot(AppDomain.CurrentDomain.BaseDirectory);

            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x => installerTypes.Contains(x) && typeof(IInstaller).IsAssignableFrom(x) &&
                !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            var commonInstallers = typeof(MyBoilerPlate.Web.Infrastructure.Installers.DefaultInstaller).Assembly.ExportedTypes.Where(x => installerTypes.Contains(x) && typeof(IInstaller).IsAssignableFrom(x) &&
                !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            var testInstallers = typeof(TestBase).Assembly.ExportedTypes.Where(x => installerTypes.Contains(x) && typeof(IInstaller).IsAssignableFrom(x) &&
                !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            if(commonInstallers.Count > 0)
                installers.AddRange(commonInstallers);

            if(testInstallers.Count > 0)
                installers.AddRange(testInstallers);

            installers.ForEach(installer => installer.InstallServices(services, configuration));

            if(installApiControllers)
            {
                var controllerBaseType = typeof(MyBoilerPlate.Web.Infrastructure.ApiControllerBase);

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
                    new Claim("sub", "D5CD2754-ED7D-4A5E-A306-11C55711E889") //Test User UNIQUE ID
                };

            var identityMock = new Mock<ClaimsIdentity>();
            identityMock.Setup(x => x.Claims).Returns(claimCollection);

            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            cp.Setup(m => m.Identity).Returns(identityMock.Object);

            cp.SetupGet(m => m.Claims).Returns(() => {
                var result = new List<Claim>();

                result.AddRange(claimCollection);

                if(this.UserClaims != null)
                    result.AddRange(this.UserClaims());

                return result;
            });

            var hc = new Mock<HttpContext>();
            hc.Setup(h => h.User).Returns(cp.Object);

            _MockHttpcontext.Setup(h => h.HttpContext).Returns(hc.Object);
            
            services.AddScoped<IUserProfile>(serviceProvider => 
            {
                var appSettings = serviceProvider.GetService<AppSettings>();

                return new UserProfile(_MockHttpcontext.Object, serviceProvider, appSettings);
            });

            services.AddScoped<IServiceProvider>(serviceProvider => serviceProvider);

            // Build the service provider
            ObjectContainer = services.BuildServiceProvider();
        }

        public void NoExceptionThrown<T>(Action codeToExecute) where T : Exception
        {
            try
            {
                codeToExecute();
            }
            catch(T ex)
            {
                Debug.WriteLine(ex.Message);

                Assert.Fail("Expected no {0} to be thrown.", typeof(T).Name);
            }
        }

        protected IUserProfile BuildUserProfile(IList<Claim> claims, List<string> permissions = null)
        {
            //Set UserProfile claims
            if(claims != null && claims.Count > 0)
                this.UserClaims = () => claims;

            if(permissions != null)
                this.UserPermissions = () => permissions.ToArray();

            return ObjectContainer.GetService<IUserProfile>();
        }

        protected void SetUserProfileProperties(IList<Claim> claims, List<string> permissions = null)
        {
            this.UserClaims = () => claims;

            if(permissions != null)
                this.UserPermissions = () => permissions.ToArray();
        }
    }
}
