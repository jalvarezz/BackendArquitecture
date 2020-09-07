using Core.Common;
using Core.Common.Contracts;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Business.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBoilerPlate.Business.Engines.Contracts
{
    public interface IEmployeeEngine : IBusinessEngine
    {
        Task<IPagedList<EmployeeDTO>> SearchAsync(string name, string officialDocumentId, int? employeeTypeId, int pageIndex, int pageSize);
    }
}