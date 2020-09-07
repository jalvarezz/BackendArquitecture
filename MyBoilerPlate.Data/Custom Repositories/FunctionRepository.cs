using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Core.Common.Base;
using MyBoilerPlate.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MyBoilerPlate.Data
{
    public class FunctionRepository : RepositoryBase<FunctionEntityBase, SampleDataContext>, IFunctionRepository
    {
        public FunctionRepository(SampleDataContext context) : base(context) { }

        public decimal CalcuateAgeInYearsMonths(DateTime startDate, DateTime endDate)
        {
            var result = _Context.Employees.Select(x => SampleDataContextFunctions.CalcuateAgeInYearsMonths(startDate, endDate)).FirstOrDefault();
            return result;
        }
    }
}
