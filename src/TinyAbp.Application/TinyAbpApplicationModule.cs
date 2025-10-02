using TinyAbp.Framework.Mapster;
using Volo.Abp.Modularity;

namespace TinyAbp.Application;

/// <summary>
/// TinyAbp应用程序模块
/// 应用程序层的主要模块配置
/// </summary>
[DependsOn(typeof(TinyAbpFrameworkDddApplicationModule), typeof(TinyAbpFrameworkMapsterModule))]
public class TinyAbpApplicationModule : AbpModule 
{ 
    /// <summary>
    /// 配置服务 - 注册应用程序依赖项
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        await base.ConfigureServicesAsync(context);
    }
}
