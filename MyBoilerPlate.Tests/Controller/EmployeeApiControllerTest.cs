using Core.Common;
using Core.Common.Contracts;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Business.Entities.DTOs;
using MyBoilerPlate.Tests.Helpers;
using MyBoilerPlate.Tests.Installers;
using MyBoilerPlate.Web.Api.Controllers;
using MyBoilerPlate.Web.Api.Infrastructure.Services;
using MyBoilerPlate.Web.Infrastructure;
using iText.Kernel.Geom;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using MyBoilerPlate.Web.Infrastructure.Models;
using MyBoilerPlate.Web.Models;
using MyBoilerPlate.Web.Infrastructure.Services;

namespace MyBoilerPlate.Tests.Controller
{
    [TestClass]
    public class EmployeeApiControllerTest : TestBase
    {
        private EmployeeApiController _Controller;
        private IBusinessEngineFactory _BusinessEngineFactory;
        private IDataRepositoryFactory _DataRepositoryFactory;

        [TestInitialize]
        public void Setup()
        {
            this.Initialize((services, configuration) =>
            {
                services.AddDefaultServices(configuration);
                services.AddHttpContextService(configuration);
                services.AddCacheService(configuration);
                services.AddDataServices(configuration);
                services.AddFakeRepositoryServices(configuration);
                services.AddEngineServices(configuration);
                services.AddHttpClientFactoryService(configuration);
                services.AddMapperService(configuration);
            }, true);

            _BusinessEngineFactory = ObjectContainer.GetService<IBusinessEngineFactory>();
            _DataRepositoryFactory = ObjectContainer.GetService<IDataRepositoryFactory>();
        }

        [TestMethod("Employee Search")]
        [DataRow("John", DisplayName = "Testing employee search for John")]
        [DataRow("Karon", DisplayName = "Testing employee search for Karen")]
        //public void GetInitialFilters(int academicYear)
        public async Task EmployeeSearch(string name)
        {
            // Arrange
            #region UserProfile (not necessary in this test)

            //NOTE: This is a mock (use it when needed to insert it to the new controller/engine instance)
            //Set UserProfile from Mock User (Set values for scenarios)
            var userProfile = this.BuildUserProfile(new List<Claim>()
            {
                new Claim("centerid", "356")
            }, new List<string>()
            {
                "my.custom.permission"
            });

            #endregion

            //Set in Controller
            _Controller = ObjectContainer.GetService<EmployeeApiController>();

            // Act 
            PagedListViewModel<EmployeeViewModel> result = await _Controller.SearchAsync(name);

            // Asserts
            Assert.IsNotNull(result.Data.Length > 0, "Should return something");
        }
        
    }
}
