using Volo.Abp.Application;
using Volo.Abp.Modularity;

/// <summary>
/// TinyAbp框架DDD应用程序契约模块
/// 定义应用程序层的契约接口和服务
/// </summary>
[DependsOn(typeof(AbpDddApplicationContractsModule))]
public class TinyAbpFrameworkDddApplicationContractsModule : AbpModule 
{ 
    /// <summary>
    /// 配置服务 - 注册DDD应用程序契约服务
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        await base.ConfigureServicesAsync(context);
    }
}
