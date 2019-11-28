using Core.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TechAssist.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SampleDataContext _DataContext;

        public UnitOfWork(SampleDataContext dataContext)
        {
            _DataContext = dataContext;
        }

        public IDbContextTransaction CreateTransaction()
        {
            return this._DataContext.Database.BeginTransaction();
        }

        /// <summary>
        /// This methods execute a transaction when working with multiple contexts.
        /// Remember that all the DB Context must have the same Connection Strings.
        /// </summary>
        /// <param name="codeToExecute"></param>
        /// <returns>T/Success - F/Fail</returns>
        public bool ExecuteMultiContextTransaction(Action codeToExecute)
        {
            using (var conn = new SqlConnection("..."))
            {
                conn.Open();

                using (var sqlTxn = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        
                        codeToExecute();

                        /* Multiple Context code can be executed in a single transaction using this method.
                         * 
                         * Example of "codeToExecute":
                         * 
                        using (var contextA = new ContextA(conn, contextOwnsConnection: false))
                        {
                            contextA.Database.UseTransaction(sqlTxn);
                            contextA.Save(...);
                            contextA.SaveChanges();
                        }

                        using (var contextB = new ContextB(conn, contextOwnsConnection: false))
                        {
                            contextB.Database.UseTransaction(sqlTxn);
                            contextB.Save(...);
                            contextB.SaveChanges();
                        }
                        */

                        sqlTxn.Commit();

                        return true;
                    }
                    catch (Exception)
                    {
                        sqlTxn.Rollback();
                    }
                }
            }

            return false;
        }

        public int SaveChanges()
        {
            return this._DataContext.SaveChanges();
        }

        public void Dispose()
        {
            if (_DataContext != null)
            {
                _DataContext.Dispose();
            }
        }
    }
}
