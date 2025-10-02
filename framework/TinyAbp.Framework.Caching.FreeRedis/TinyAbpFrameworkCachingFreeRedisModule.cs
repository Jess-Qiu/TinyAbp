using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace TinyAbp.Framework.Caching.FreeRedis;

/// <summary>
/// TinyAbp Framework FreeRedis 缓存模块
/// 集成 FreeRedis 作为分布式缓存提供程序
/// </summary>
[DependsOn(typeof(AbpCachingModule))]
public class TinyAbpFrameworkCachingFreeRedisModule : AbpModule
{
    /// <summary>
    /// 配置服务 - 注册FreeRedis缓存相关服务
    /// </summary>
    /// <param name="context">服务配置上下文</param>
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
