using MyBoilerPlate.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Data.Initializers
{
    public class EmployeeTypeInitializer
    {
        private readonly SampleDataContext _Context;

        public EmployeeTypeInitializer(SampleDataContext context)
        {
            _Context = context;
        }

        public async Task Seed()
        {
            if (!_Context.EmployeeTypes.Any())
            {
                _Context.EmployeeTypes.Add(new EmployeeType
                {
                    Name = "Manager",
                    Deferred = false
                });

                _Context.EmployeeTypes.Add(new EmployeeType
                {
                    Name = "Director",
                    Deferred = false
                });

                await _Context.SaveChangesAsync();
            }
        }
    }
}
