using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace TinyAbp.Framework.Caching.FreeRedis;

[DependsOn(typeof(AbpCachingModule))]
public class TinyAbpFrameworkCachingFreeRedisModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        var redisEnabled = configuration["Redis:IsEnabled"];
        if (string.IsNullOrEmpty(redisEnabled) || bool.Parse(redisEnabled))
        {
            var redisConfiguration = configuration["Redis:Configuration"];
            if (!redisConfiguration.IsNullOrEmpty())
            {
                var redisClient = new RedisClient(redisConfiguration);

                context.Services.AddSingleton<IRedisClient>(redisClient);
                context.Services.Replace(
                    ServiceDescriptor.Singleton<IDistributedCache>(
                        new DistributedCache(redisClient)
                    )
                );
            }
            ;
        }
    }
}
