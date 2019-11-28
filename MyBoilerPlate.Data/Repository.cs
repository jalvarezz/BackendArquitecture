using Core.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechAssist.Data
{
    /// <summary>
    /// Main repository class. This class is linked to a DbContext.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : RepositoryBase<TEntity, SampleDataContext> where TEntity : class, new()
    {
        public Repository(SampleDataContext context) : base(context) {  }
    }
}
