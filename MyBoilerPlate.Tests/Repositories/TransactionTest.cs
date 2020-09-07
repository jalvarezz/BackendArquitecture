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
using MyBoilerPlate.Tests.Installers;

namespace MyBoilerPlate.Tests.Repositories
{
    [TestClass]
    public class RepositoryTest : TestBase
    {
        private IDataRepositoryFactory _DataRepositoryFactory;

        [TestInitialize]
        public void Setup()
        {
            this.Initialize(new List<Type>()
            {
                typeof(HttpContextInstaller),
                typeof(DataInstaller),
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
