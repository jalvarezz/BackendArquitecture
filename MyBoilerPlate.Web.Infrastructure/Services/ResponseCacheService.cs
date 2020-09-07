using MyBoilerPlate.Web.Infrastructure.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MyBoilerPlate.Web.Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _DistributedCache;
        private readonly RedisCacheSettings _Settings;

        public ResponseCacheService(IDistributedCache distributedCache, RedisCacheSettings _settings)
        {
            _DistributedCache = distributedCache;
            _Settings = _settings;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan expirationTime)
        {
            if(response == null || !_Settings.Enabled)
                return;

            var serializeResponse = JsonConvert.SerializeObject(response);

            await _DistributedCache.SetStringAsync(cacheKey, serializeResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime
            });
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            if(!_Settings.Enabled)
                return null;

            var cachedResponse = await _DistributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}
