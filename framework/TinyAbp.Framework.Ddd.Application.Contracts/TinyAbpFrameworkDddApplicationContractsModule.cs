using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace TinyAbp.Framework.Ddd.Application.Contracts;

[DependsOn(typeof(AbpDddApplicationContractsModule))]
public class TinyAbpFrameworkDddApplicationContractsModule : AbpModule { }
