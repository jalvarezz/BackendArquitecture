using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

using System.Text;

namespace Core.Common.Contracts
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// This creates a transaction for the datacontext injected in the Unit of Work concrete object
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction CreateTransaction();

        /// <summary>
        /// This methods execute a transaction when working with multiple contexts.
        /// Remember that all the DB Context must have the same Connection Strings.
        /// </summary>
        /// <param name="codeToExecute"></param>
        /// <returns>T/Success - F/Fail</returns>
        bool ExecuteMultiContextTransaction(Action codeToExecute);

        int SaveChanges();
    }
}
