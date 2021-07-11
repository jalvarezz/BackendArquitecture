using System;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface ICacheService
    {
        ValueTask<T> GetCachedAsync<T>(string key);

        ValueTask<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer);

        ValueTask<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer, TimeSpan slidingExpiration);

        ValueTask AppendToCachedListAsync<T>(string key, T item);

        ValueTask ClearAsync(string key);
    }
}