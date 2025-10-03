using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

/// <summary>
/// TinyAbp框架DDD应用程序模块
/// 负责配置和注册DDD应用程序层相关服务
/// </summary>
[DependsOn(typeof(TinyAbpFrameworkDddApplicationContractsModule), typeof(AbpDddApplicationModule))]
public class TinyAbpFrameworkDddApplicationModule : AbpModule
{
    /// <summary>
    /// 配置服务 - 注册DDD应用程序层服务
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        ConfigureDistributedLocking(context);

        await base.ConfigureServicesAsync(context);
    }

    private void ConfigureDistributedLocking(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var redisConfiguration = configuration["Redis:Configuration"];

        if (!redisConfiguration.IsNullOrWhiteSpace())
        {
            context.Services.AddSingleton<IDistributedLockProvider>(sp =>
            {
                var connection = ConnectionMultiplexer.Connect(redisConfiguration);

                return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
            });
        }
    }
}
