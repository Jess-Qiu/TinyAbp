using TinyAbp.Framework.Ddd.Application.Contracts;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace TinyAbp.Framework.Ddd.Application;

[DependsOn(typeof(TinyAbpFrameworkDddApplicationContractsModule), typeof(AbpDddApplicationModule))]
public class TinyAbpFrameworkDddApplicationModule : AbpModule { }
