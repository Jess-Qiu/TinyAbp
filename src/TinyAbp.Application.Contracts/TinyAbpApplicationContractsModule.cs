using TinyAbp.Framework.Ddd.Application.Contracts;
using Volo.Abp.Modularity;

namespace TinyAbp.Application.Contracts;

[DependsOn(typeof(TinyAbpFrameworkDddApplicationContractsModule))]
public class TinyAbpApplicationContractsModule : AbpModule { }
