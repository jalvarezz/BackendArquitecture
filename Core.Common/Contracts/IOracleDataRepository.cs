using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface IOracleDataRepository : IDataRepository
    {
    }

    public interface IOracleDataRepository<T> : IDataRepository<T>
         where T : class, new()
    {
    }
}
