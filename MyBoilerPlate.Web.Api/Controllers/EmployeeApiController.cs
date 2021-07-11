using AutoMapper;
using Core.Common;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Business.Entities.DTOs;
using MyBoilerPlate.Data.Contracts;
using MyBoilerPlate.Web.Infrastructure;
using MyBoilerPlate.Web.Infrastructure.Settings;
using MyBoilerPlate.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBoilerPlate.Web.Infrastructure.Models;

namespace MyBoilerPlate.Web.Api.Controllers
{
    [Authorize]
    [Route("api/student")]
    public class EmployeeApiController : ApiControllerBase
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        private readonly IBusinessEngineFactory _BusinessEngineFactory;
        private readonly IMessageHandler _MessageHandler;
        private readonly IMapper _Mapper;
        private readonly AppSettings _AppSettings;
        private readonly IUserProfile _UserProfile;

        public EmployeeApiController(IDataRepositoryFactory dataRepositoryFactory,
                                    IBusinessEngineFactory businessEngineFactory,
                                    IMessageHandler messageHandler,
                                    IMapper mapper,
                                    IUserProfile userProfile,  //NOTE: Inject the UserProfile object when your need to acces a property of the logged in user
                                    AppSettings appSettings) //You can also inject configuration directly to the controller, or engine or wherever
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
            _MessageHandler = messageHandler;
            _Mapper = mapper;
            _UserProfile = userProfile;
            _AppSettings = appSettings;
        }

        [Route("")]
        [HttpGet]
        public async Task<PagedListViewModel<EmployeeViewModel>> SearchAsync([FromQuery]string name = null, [FromQuery]string officialDocumentId = null, [FromQuery]int? employeeTypeId = null, [FromQuery]int pageIndex = 1, [FromQuery]int pageSize = 5)
        {
            var engine = _BusinessEngineFactory.GetBusinessEngine<IEmployeeEngine>();

            var data = await engine.SearchAsync(name, officialDocumentId, employeeTypeId, pageIndex, pageSize);

            var result = _Mapper.Map<PagedListViewModel<EmployeeViewModel>>(data);

            return result;
        }

        [Route("")]
        [HttpPost]
        public async Task<EmployeeViewModel> Post([FromBody]EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Error in parameters");

            var result = new EmployeeViewModel();

            //NOTE: Use transactions where there are multiple I/O operations, this time is used
            // for demo purpose
            //NOTE 2: Always start it in the controllers to prevent nested transactions
            using (var trans = _DataRepositoryFactory.GetUnitOfWork().CreateTransaction())
            {
                var repo = _DataRepositoryFactory.GetDataRepository<Employee>();

                //This whas done this way for teaching purpose
                var dto = _Mapper.Map<EmployeeDTO>(model);
                var entity = _Mapper.Map<Employee>(dto);

                entity = await repo.AddAsync(entity);

                result = _Mapper.Map<EmployeeViewModel>(_Mapper.Map<EmployeeDTO>(entity));

                await trans.CommitAsync();
            }

            return result;
        }

        [Route("")]
        [HttpPut]
        public async Task<EmployeeViewModel> Put([FromBody]EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Error in parameters");

            var result = new EmployeeViewModel();

            //NOTE: Use transactions where there are multiple I/O operations, this time is used
            // for demo purpose
            //NOTE 2: Always start it in the controllers to prevent nested transactions
            using (var trans = _DataRepositoryFactory.GetUnitOfWork().CreateTransaction())
            {
                var repo = _DataRepositoryFactory.GetDataRepository<Employee>();

                //This whas done this way for teaching purpose
                var dto = _Mapper.Map<EmployeeDTO>(model);
                var entity = _Mapper.Map<Employee>(dto);

                entity = await repo.UpdateAsync(entity);

                result = _Mapper.Map<EmployeeViewModel>(_Mapper.Map<EmployeeDTO>(entity));

                await trans.CommitAsync();
            }

            return result;
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            if (id == 0)
                throw new ArgumentException("Error in parameters");

            //NOTE: Use transactions where there are multiple I/O operations, this time is used
            // for demo purpose
            //NOTE 2: Always start it in the controllers to prevent nested transactions
            using (var trans = _DataRepositoryFactory.GetUnitOfWork().CreateTransaction())
            {
                var repo = _DataRepositoryFactory.GetDataRepository<Employee>();

                var entity = await repo.GetAsync(x => x.Select(r => r), x => x.Id == id);

                await repo.RemoveAsync(entity);
                await trans.CommitAsync();
            }
        }
    }
}