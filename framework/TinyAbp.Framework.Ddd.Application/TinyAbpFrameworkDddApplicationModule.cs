using TinyAbp.Framework.Ddd.Application.Contracts;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace TinyAbp.Framework.Ddd.Application;

/// <summary>
/// TinyAbp框架DDD应用程序模块
/// 负责配置和注册DDD应用程序层相关服务
/// </summary>
[DependsOn(typeof(TinyAbpFrameworkDddApplicationContractsModule), typeof(AbpDddApplicationModule))]
public class TinyAbpFrameworkDddApplicationModule : AbpModule { }
