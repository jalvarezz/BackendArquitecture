using MyBoilerPlate.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Data.Initializers
{
    public class EmployeeInitializer
    {
        private readonly SampleDataContext _Context;

        public EmployeeInitializer(SampleDataContext context)
        {
            _Context = context;
        }

        public async Task Seed()
        {
            if (!_Context.Employees.Any())
            {
                _Context.Employees.Add(new Employee
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmployeeTypeId = 1,
                    Deferred = false
                });

                _Context.Employees.Add(new Employee
                {
                    FirstName = "Karen",
                    LastName = "Diaz",
                    EmployeeTypeId = 2,
                    Deferred = false
                });

                await _Context.SaveChangesAsync();
            }
        }
    }
}
