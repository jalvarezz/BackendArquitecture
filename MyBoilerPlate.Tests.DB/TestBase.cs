using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Core.Common.Contracts;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using EduEsp.Web;
using System.Linq;
using EduEsp.Web.Api;
using System.Collections.Generic;
using EduEsp.Web.Infrastructure.Installers;
using EduEsp.Web.Infrastructure;

namespace EduEsp.Tests
{
    public abstract class TestBase
    {
        protected ServiceProvider ObjectContainer { get; private set; }

        protected void Initialize(IEnumerable<Type> installerTypes)
        {
            var services = new ServiceCollection();
            var configuration = TestConfigurationBuilder.GetIConfigurationRoot(AppDomain.CurrentDomain.BaseDirectory);

            var commonInstallers = typeof(DefaultInstaller).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();

            commonInstallers.ForEach(installer => installer.InstallServices(services, configuration));

            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x => installerTypes.Contains(x) &&
                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));

            services.AddOptions();

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

            NoExceptionThrown<Exception>(async () => { await repo.GetSingleAsync(x => x); });
        }
    }
}
