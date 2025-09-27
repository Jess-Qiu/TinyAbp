using Volo.Abp;
using Volo.Abp.Modularity;

namespace TinyAbp.HttpApi.Host;

/// <summary>
/// Tiny Abp HttpApi Host Module - 主应用程序模块配置
/// </summary>
public class TinyAbpHttpApiHostModule : AbpModule
{
    /// <summary>
    /// 预配置服务 - 在ConfigureServices之前执行
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
        await base.PreConfigureServicesAsync(context);
    }

    /// <summary>
    /// 配置服务 - 注册应用程序依赖项
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        await base.ConfigureServicesAsync(context);
    }

    /// <summary>
    /// 应用程序初始化 - 配置中间件管道
    /// </summary>
    /// <param name="context">应用程序初始化上下文</param>
    /// <returns></returns>
    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context
    )
    {
        await base.OnApplicationInitializationAsync(context);
    }
}
