using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBoilerPlate.Web.Infrastructure.Services.Contracts
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);

        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
