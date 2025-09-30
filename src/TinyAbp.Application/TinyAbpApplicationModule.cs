using TinyAbp.Framework.Mapster;
using Volo.Abp.Modularity;

namespace TinyAbp.Application;

/// <summary>
/// TinyAbp应用程序模块
/// 应用程序层的主要模块配置
/// </summary>
[DependsOn(typeof(TinyAbpFrameworkDddApplicationModule), typeof(TinyAbpFrameworkMapsterModule))]
public class TinyAbpApplicationModule : AbpModule { }
