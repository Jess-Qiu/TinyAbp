using TinyAbp.Framework.Ddd.Application;
using Volo.Abp.Modularity;

namespace TinyAbp.Application;

/// <summary>
/// TinyAbp应用程序模块
/// 应用程序层的主要模块配置
/// </summary>
[DependsOn(typeof(TinyAbpFrameworkDddApplicationModule))]
public class TinyAbpApplicationModule : AbpModule { }
