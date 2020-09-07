using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Common.Contracts
{
    public interface IUnitOfWork
    {
        IDbContextTransaction CreateTransaction();

        int SaveChanges();
    }
}
