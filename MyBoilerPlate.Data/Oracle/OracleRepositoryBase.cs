using Core.Common.Base;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Data.Oracle
{
    public class OracleRepositoryBase<TEntity> : RepositoryBase<TEntity, SampleOracleDataContext>, IOracleDataRepository<TEntity>
          where TEntity : class, new()
    {
        protected readonly SampleOracleDataContext _Context;

        public OracleRepositoryBase(SampleOracleDataContext context) : base(context)
        {
            _Context = context;
        }
    }
}
