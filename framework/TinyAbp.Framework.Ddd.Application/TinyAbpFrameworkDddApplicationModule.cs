using Volo.Abp.Application;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

/// <summary>
/// TinyAbp框架DDD应用程序模块
/// 负责配置和注册DDD应用程序层相关服务
/// </summary>
[DependsOn(
    typeof(TinyAbpFrameworkDddApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpFluentValidationModule)
)]
public class TinyAbpFrameworkDddApplicationModule : AbpModule { }
