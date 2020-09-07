namespace Core.Common.Contracts
{
   public interface IContigencyDataRepository<T> : IDataRepository<T>
        where T : class, new()
    {
    }
}
