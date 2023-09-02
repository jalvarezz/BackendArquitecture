using Core.Common.Contracts;
using Moq;
using System;
using System.Linq;
using MyBoilerPlate.Data;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Business.Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Entities.DTOs;
using System.Collections.Generic;
using MyBoilerPlate.Business;
using MyBoilerPlate.Web.Api.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using MyBoilerPlate.Web.Infrastructure.Services;
using MyBoilerPlate.Tests.Installers;

namespace MyBoilerPlate.Tests.Repositories
{
    [TestClass]
    public class FactoryTest : TestBase
    {
        private IDataRepositoryFactory _DataRepositoryFactory;

        [TestInitialize]
        public void Setup()
        {
            this.Initialize((services, configuration) =>
            {
                services.AddHttpContextService(configuration);
                services.AddDataServices(configuration);
                services.AddFakeRepositoryServices(configuration);
            });

            _DataRepositoryFactory = ObjectContainer.GetService<IDataRepositoryFactory>();
        }

        [TestMethod]
        public async Task GetPeiRecord()
        {
            // Arrange
            var id = 1;
            var repo = _DataRepositoryFactory.GetDataRepository<Employee>();

            // Act
            var data = await repo.GetAllAsync(x => x.Select(r => r), x => x.Id == id);

            // Assert
            Assert.IsNotNull(data);
        }
    }
}
