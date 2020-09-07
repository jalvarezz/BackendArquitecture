using Core.Common.Base;
using Core.Common.Contracts;
using System;
using System.Threading.Tasks;

namespace MyBoilerPlate.Data.Contracts
{
    public interface IFunctionRepository : IDataRepository<FunctionEntityBase>
    {
        decimal CalcuateAgeInYearsMonths(DateTime startDate, DateTime endDate);
    }
}
