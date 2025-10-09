using Volo.Abp.Modularity;

namespace TinyAbp.Domain.Shared;

/// <summary>
/// TinyAbp域共享模块
/// 提供领域层的共享常量、枚举和基础类型定义
/// </summary>
public class TinyAbpDomainSharedModule : AbpModule
{
    /// <summary>
    /// 配置服务 - 注册域共享服务
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        await base.ConfigureServicesAsync(context);
    }
}
