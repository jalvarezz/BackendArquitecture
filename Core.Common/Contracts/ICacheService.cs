using System;
using System.Threading.Tasks;

namespace Core.Common.Contracts
{
    public interface ICacheService
    {
        T GetCached<T>(string key);

        Task<T> GetCachedAsync<T>(string key);

        T GetCached<T>(string key, Func<T> initializer);

        Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer);

        T GetCached<T>(string key, Func<T> initializer, TimeSpan slidingExpiration);

        Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer, TimeSpan slidingExpiration);

        void AppendToCachedList<T>(string key, T item);

        Task AppendToCachedListAsync<T>(string key, T item);

        Task Clear(string key);
    }
}