using Core.Common.Contracts;
using Moq;
using System;
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
using MyBoilerPlate.Data.Contracts;
using MyBoilerPlate.Web.Infrastructure.Installers;

namespace MyBoilerPlate.Tests.Repositories
{
    [TestClass]
    public class FunctionTest : TestBase
    {
        private IFunctionRepository _Repository;

        [TestInitialize]
        public void Setup()
        {
            this.Initialize(new List<Type>()
            {
                typeof(HttpContextInstaller),
                typeof(DataInstaller),
                typeof(RepositoryInstaller)
            });

            _Repository = ObjectContainer.GetService<IFunctionRepository>();
        }

        [TestMethod]
        public void Function_CalcuateAgeInYearsMonths()
        {
            // Arrange
            DateTime startDate = new DateTime(1987, 1, 4);
            DateTime endDate = DateTime.Now;
           
            // Act
            var data = _Repository.CalcuateAgeInYearsMonths(startDate, endDate);

            // Assert 
            Assert.IsTrue(data > 0);
        }
    }
}
