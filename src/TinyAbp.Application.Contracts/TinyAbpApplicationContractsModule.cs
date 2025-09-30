using Volo.Abp.Modularity;

namespace TinyAbp.Application.Contracts;

/// <summary>
/// TinyAbp应用程序契约模块
/// 定义应用程序层的契约接口
/// </summary>
[DependsOn(typeof(TinyAbpFrameworkDddApplicationContractsModule))]
public class TinyAbpApplicationContractsModule : AbpModule { }
