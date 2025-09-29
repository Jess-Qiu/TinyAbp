using TinyAbp.Framework.Ddd.Application;
using Volo.Abp.Modularity;

namespace TinyAbp.Application;

[DependsOn(typeof(TinyAbpFrameworkDddApplicationModule))]
public class TinyAbpApplicationModule : AbpModule { }
