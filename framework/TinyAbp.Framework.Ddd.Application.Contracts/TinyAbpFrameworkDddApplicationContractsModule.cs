using Volo.Abp.Application;
using Volo.Abp.Modularity;

/// <summary>
/// TinyAbp框架DDD应用程序契约模块
/// 定义应用程序层的契约接口和服务
/// </summary>
[DependsOn(typeof(AbpDddApplicationContractsModule))]
public class TinyAbpFrameworkDddApplicationContractsModule : AbpModule { }
