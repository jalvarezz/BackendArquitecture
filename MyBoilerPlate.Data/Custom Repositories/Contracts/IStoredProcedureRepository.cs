using Core.Common;
using Core.Common.Base;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBoilerPlate.Data.Contracts
{
    public interface IStoredProcedureRepository : IDataRepository<StoredProcedureEntityBase>
    {
        //IEnumerable<TSPEntity> SampleProcedureCall(bool sampleParameter);
    }
}
