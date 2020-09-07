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
using MyBoilerPlate.Web.Api.Infrastructure.Installers;
using Microsoft.Extensions.DependencyInjection;
using MyBoilerPlate.Web.Infrastructure.Installers;
using System.Threading.Tasks;

namespace MyBoilerPlate.Tests.Repositories
{
    [TestClass]
    public class FactoryTest : TestBase
    {
        private IDataRepositoryFactory _DataRepositoryFactory;

        [TestInitialize]
        public void Setup()
        {
            this.Initialize(new List<Type>()
            {
                typeof(HttpContextInstaller),
                typeof(DataInstaller),
                typeof(RepositoryInstaller)
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
