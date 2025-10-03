using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace TinyAbp.Application.Contracts;

/// <summary>
/// TinyAbp应用程序契约模块
/// 定义应用程序层的契约接口
/// </summary>
[DependsOn(typeof(TinyAbpFrameworkDddApplicationContractsModule))]
public class TinyAbpApplicationContractsModule : AbpModule
{
    /// <summary>
    /// 配置服务 - 注册应用程序契约服务
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        await base.ConfigureServicesAsync(context);
    }
}
