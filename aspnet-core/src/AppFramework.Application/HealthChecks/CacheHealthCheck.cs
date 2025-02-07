using System;
using System.Threading;
using System.Threading.Tasks;
using Abp.Runtime.Caching;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AppFramework.HealthChecks
{
    public class CacheHealthCheck : IHealthCheck
    {
        private readonly ICacheManager _cacheManager;

        public CacheHealthCheck(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        // This function tries to set and get data from cache.
        // If redis cache is enabled this will try to connect to redis to set and get cache data. If it will not throw an exception it means redis is up and healthy.
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var cacheManager = _cacheManager.GetCache("TestCache");

                var testKey = "Test-" + Guid.NewGuid();

                await cacheManager.SetAsync(testKey, "123");
                
                await cacheManager.GetOrDefaultAsync(testKey);

                return HealthCheckResult.Healthy("The cache check is healthy. (If you are using Redis, Redis is also checked)");
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy("The cache check is unhealthy. (If you are using Redis, Redis is also checked)" + e.Message);
            }
        }
    }
}
