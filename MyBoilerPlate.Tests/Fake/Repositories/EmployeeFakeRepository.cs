﻿using Core.Common.Base;
using MyBoilerPlate.Data;
using MyBoilerPlate.Tests.Fake;
using MyBoilerPlate.Business.Entities;
using MyBoilerPlate.Tests.Helpers;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyBoilerPlate.Tests
{
    public class EmployeeFakeRepository : FakeRepositoryBase<Employee>
    {
        public EmployeeFakeRepository(SampleDataContext context) : base(context) 
        {
#if DEBUG
            //Valid if file exists for generated locally before committed
            if(!Record.Exists<Employee>())
            {
                //Load test data
                //NOTE: Remember to filter only the data that you need to test
                this.FakeData = context.Employees.Select(y => new Employee
                {
                    Id = y.Id,
                    EmployeeTypeId = y.EmployeeTypeId,
                    FirstName = y.FirstName,
                    LastName = y.LastName,
                    OfficialDocumentId = y.OfficialDocumentId,
                    EmployeeType = new EmployeeType
                    {
                        Id = y.EmployeeType.Id,
                        Name = y.EmployeeType.Name
                    }
                }).ToList();

                //Record the data
                Record.RecordData(this.FakeData);
            }
#endif

            if(!this.FakeData.Any())
                this.FakeData = Record.LoadData<Employee>().ToList();
        }
    }
}
