using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Core.Common.Contracts;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MyBoilerPlate.Web;
using System.Linq;
using MyBoilerPlate.Web.Api;
using System.Collections.Generic;
using MyBoilerPlate.Web.Infrastructure;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MyBoilerPlate.Web.Infrastructure.Settings;
using System.Threading.Tasks;
using MyBoilerPlate.Web.Infrastructure.Services;

namespace MyBoilerPlate.Tests
{
    public abstract class TestBase
    {
        private Mock<IHttpContextAccessor> _MockHttpcontext;

        protected ServiceProvider ObjectContainer { get; private set; }

        protected void Initialize(IEnumerable<Type> installerTypes)
        {
            var services = new ServiceCollection();
            var configuration = TestConfigurationBuilder.GetIConfigurationRoot(AppDomain.CurrentDomain.BaseDirectory);

            var commonInstallers = typeof(DefaultService).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();

            commonInstallers.ForEach(installer => installer.InstallServices(services, configuration));

            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x => installerTypes.Contains(x) &&
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));

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

                return result;
            });

            var hc = new Mock<HttpContext>();
            hc.Setup(h => h.User).Returns(cp.Object);

            _MockHttpcontext.Setup(h => h.HttpContext).Returns(hc.Object);

            services.AddScoped<IUserProfile>(serviceProvider =>
            {
                return new UserProfile(_MockHttpcontext.Object, serviceProvider);
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

        public void TestRepo<T>() where T : class, new()
        {
            IDataRepository<T> repo = ObjectContainer.GetService<IDataRepository<T>>();

            Debug.WriteLine($"Testing {typeof(T).Name} Repository");

            NoExceptionThrown<Exception>(() => {
                repo.GetAsync(x => x).GetAwaiter().GetResult();
            });
        }
    }
}
