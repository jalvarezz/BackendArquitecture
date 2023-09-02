using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Data.Oracle
{
    public class OracleRepository<TEntity> : OracleRepositoryBase<TEntity> where TEntity : class, new()
    {
        public OracleRepository(SampleOracleDataContext context) : base(context) { }
    }
}
