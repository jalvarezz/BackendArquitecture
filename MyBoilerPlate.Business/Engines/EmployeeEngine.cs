using Core.Common.Contracts;
using MyBoilerPlate.Business.Contracts;
using MyBoilerPlate.Business.Engines.Contracts;
using MyBoilerPlate.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using Core.Common.Extensions;
using Core.Common;
using System.Threading.Tasks;
using MyBoilerPlate.Business.Entities.DTOs;

namespace MyBoilerPlate.Business.Engines
{
    public class EmployeeEngine : IEmployeeEngine
    {
        private readonly IDataRepositoryFactory _DataRepositoryFactory;
        private readonly IBusinessEngineFactory _BusinessEngineFactory;
        private readonly IMessageHandler _MessageHandler;

        public EmployeeEngine(IDataRepositoryFactory dataRepositoryFactory,
                                     IBusinessEngineFactory businessEngineFactory,
                                     IMessageHandler messageHandler)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
            _MessageHandler = messageHandler;
        }

        public async Task<IPagedList<EmployeeDTO>> SearchAsync(string name, string officialDocumentId, int? employeeTypeId, int pageIndex, int pageSize)
        {
            var repo = _DataRepositoryFactory.GetDataRepository<Employee>();

            var predicate = PredicateBuilder.New<Employee>();

            if (!string.IsNullOrEmpty(name))
            {
                predicate.And(r => name.Contains(r.FirstName) || name.Contains(r.LastName));
            }

            if (!string.IsNullOrEmpty(officialDocumentId))
            {
                predicate.And(r => r.OfficialDocumentId == officialDocumentId);
            }

            if(employeeTypeId.HasValue)
            {
                predicate.And(r => r.EmployeeTypeId == employeeTypeId);
            }

            var result = await repo.GetPagedAsync(x => x.Select(r => new EmployeeDTO
            {
                FullName = $"{r.FirstName} {r.LastName}",
                EmployeeTypeId = r.EmployeeTypeId.Value,
                OfficialDocumentId = r.OfficialDocumentId,
                EmployeeType = new EmployeeTypeDTO
                {
                    Id = r.EmployeeType.Id,
                    Name = r.EmployeeType.Name
                }
            }), predicate, pageIndex, pageSize);

            return result;
        }
    }
}
