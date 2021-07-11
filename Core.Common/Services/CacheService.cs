using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Core.Common.Extensions;
using Core.Common.Contracts;
using System.Threading.Tasks;
using Core.Common.Settings;
using System.Text.Json;
using Serilog;

namespace Core.Common.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _Cache;
        private readonly bool _IsCacheEnabled;
        private readonly JsonSerializerOptions _JsonSerializerOptions;

        public CacheService(IDistributedCache cache, RedisCacheSettings settings, JsonSerializerOptions jsonSerializerOptions)
        {
            _Cache = cache;
            _IsCacheEnabled = settings != null ? settings.Enabled : false;
            _JsonSerializerOptions = jsonSerializerOptions;
        }

        public async ValueTask AppendToCachedListAsync<T>(string key, T item)
        {
            if (!_IsCacheEnabled)
                return;

            var obj = await _Cache.GetAsync(key);

            if (obj != null)
            {
                List<T> list = obj as List<T>;

                list.Add(item);
            }
        }

        public async ValueTask<T> GetCachedAsync<T>(string key)
        {
            T result = default(T);

            if (!_IsCacheEnabled)
                return result;

            var obj = await _Cache.GetAsync(key);

            if (obj != null)
            {
                return obj.DeserializeFromBytes<T>();
            }

            //Nothing was found
            return default(T);
        }

        public async ValueTask<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer)
        {
            return await GetCachedAsync(key, initializer, TimeSpan.Zero);
        }

        public async ValueTask<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer, TimeSpan slidingExpiration)
        {
            T result = default(T);

            if (!_IsCacheEnabled)
            {
                result = await initializer();

                return result;
            }

            var chacheData = await _Cache.GetStringAsync(key);

            if (chacheData == null)
            {
                var options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = slidingExpiration
                };

                result = await initializer();

                await _Cache.SetStringAsync(key, JsonSerializer.Serialize(result, _JsonSerializerOptions), options);
            }
            else
            {
                result = JsonSerializer.Deserialize<T>(chacheData, _JsonSerializerOptions);
            }

            // taking care of value types
            if (result == null && (typeof(T)).IsValueType)
            {
                return default(T);
            }

            return result;
        }

        public async ValueTask ClearAsync(string key)
        {
            if (_IsCacheEnabled)
                await _Cache.RemoveAsync(key);
        }
    }
}
