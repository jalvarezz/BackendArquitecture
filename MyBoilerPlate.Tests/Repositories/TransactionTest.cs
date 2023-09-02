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
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using MyBoilerPlate.Tests.Installers;
using MyBoilerPlate.Web.Infrastructure.Services;

namespace MyBoilerPlate.Tests.Repositories
{
    [TestClass]
    public class RepositoryTest : TestBase
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
        public void GetUnitOfWork()
        {
            var unitOfWork = _DataRepositoryFactory.GetUnitOfWork();

            using (var transaction = unitOfWork.CreateTransaction())
            {
                //Call whatever engine or ropository here and perform the operations

                transaction.Commit();
            }

            Assert.IsNull(null);
        }
    }
}
