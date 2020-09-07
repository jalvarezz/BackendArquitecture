using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using Core.Common.Extensions;
using Newtonsoft.Json;
using Core.Common.Contracts;
using System.Threading.Tasks;
using MyBoilerPlate.Web.Infrastructure.Settings;

namespace MyBoilerPlate.Web.Services
{
    //Note: This cache service uses Redis
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _Cache;
        private readonly object _ThisLock = new object();
        private readonly bool _IsCacheEnabled;

        public CacheService(IDistributedCache cache, RedisCacheSettings settings)
        {
            _Cache = cache;
            _IsCacheEnabled = settings != null ? settings.Enabled : false;
        }

        public void AppendToCachedList<T>(string key, T item)
        {
            if(!_IsCacheEnabled)
                return;

            var obj = _Cache.Get(key);

            if(obj != null)
            {
                List<T> list = obj as List<T>;

                list.Add(item);
            }
        }

        public async Task AppendToCachedListAsync<T>(string key, T item)
        {
            if(!_IsCacheEnabled)
                return;

            var obj = await _Cache.GetAsync(key);

            if(obj != null)
            {
                List<T> list = obj as List<T>;

                list.Add(item);
            }
        }

        public T GetCached<T>(string key)
        {
            if(!_IsCacheEnabled)
                return default(T);

            var obj = _Cache.Get(key);

            if(obj != null)
            {
                return obj.DeserializeFromBytes<T>();
            }

            //Nothing was found
            return default(T);
        }

        public async Task<T> GetCachedAsync<T>(string key)
        {
            T result = default(T);

            if(!_IsCacheEnabled)
                return result;

            var obj = await _Cache.GetAsync(key);

            if(obj != null)
            {
                return obj.DeserializeFromBytes<T>();
            }

            //Nothing was found
            return default(T);
        }

        public T GetCached<T>(string key, Func<T> initializer)
        {
            return GetCached(key, initializer, TimeSpan.Zero);
        }

        public async Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer)
        {
            return await GetCachedAsync(key, initializer, TimeSpan.Zero);
        }

        public T GetCached<T>(string key, Func<T> initializer, TimeSpan slidingExpiration)
        {
            if(!_IsCacheEnabled)
                return initializer();

            T result = default(T);

            var chacheData = _Cache.GetString(key);

            if(chacheData == null)
            {
                var options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = slidingExpiration
                };

                result = initializer();

                _Cache.SetString(key, JsonConvert.SerializeObject(result), options);
            }
            else
            {
                result = JsonConvert.DeserializeObject<T>(chacheData);
            }

            // taking care of value types
            if(chacheData == null && (typeof(T)).IsValueType)
            {
                return default(T);
            }

            return result;
        }

        public async Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer, TimeSpan slidingExpiration)
        {
            if(!_IsCacheEnabled)
                return await initializer();

            T result = default(T);

            var chacheData = await _Cache.GetStringAsync(key);

            if(chacheData == null)
            {
                var options = new DistributedCacheEntryOptions
                {
                    SlidingExpiration = slidingExpiration
                };

                result = await initializer();

                await _Cache.SetStringAsync(key, JsonConvert.SerializeObject(result), options);
            }
            else
            {
                result = JsonConvert.DeserializeObject<T>(chacheData);
            }

            // taking care of value types
            if(chacheData == null && (typeof(T)).IsValueType)
            {
                return default(T);
            }

            return result;
        }

        public async Task Clear(string key)
        {
            await _Cache.RemoveAsync(key);
        }
    }
}
