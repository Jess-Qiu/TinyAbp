using Volo.Abp.Modularity;

namespace TinyAbp.Domain;

/// <summary>
/// TinyAbp域模块
/// 负责领域层核心业务逻辑的配置和管理
/// </summary>
public class TinyAbpDomainModule : AbpModule
{
    /// <summary>
    /// 配置服务 - 注册领域层依赖项
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        await base.ConfigureServicesAsync(context);
    }
}
