using Core.Common.Contracts;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Data.Oracle
{
    public interface IOracleUnitOfWork : IUnitOfWork
    {

    }

    public class OracleUnitOfWork : IOracleUnitOfWork, IDisposable
    {
        // Track whether Dispose has been called.
        private bool _Disposed = false;

        private readonly SampleOracleDataContext _DataContext;

        public OracleUnitOfWork(SampleOracleDataContext dataContext)
        {
            _DataContext = dataContext;
        }

        public IDbContextTransaction CreateTransaction()
        {
            return this._DataContext.Database.BeginTransaction();
        }

        public int SaveChanges()
        {
            return _DataContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._Disposed)
            {
                if (disposing && _DataContext != null)
                {
                    _DataContext.Dispose();
                }

                _Disposed = true;
            }
        }
    }
}
