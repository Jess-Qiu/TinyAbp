using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TinyAbp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace TinyAbp.Framework.AspNetCore;

/// <summary>
/// Tiny Abp Framework AspNetCore Module - 集成ASP.NET Core相关功能
/// </summary>
[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreSerilogModule)
)]
public class TinyAbpFrameworkAspNetCoreModule : AbpModule
{
    /// <summary>
    /// 配置服务 - 注册应用程序依赖项
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        // 替换默认的路由构建器为自定义的TinyAbpConventionalRouteBuilder
        context.Services.Replace(
            ServiceDescriptor.Transient<
                IConventionalRouteBuilder,
                TinyAbpConventionalRouteBuilder
            >()
        );

        // 替换默认的服务约定为自定义的TinyAbpServiceConvention
        context.Services.Replace(
            ServiceDescriptor.Transient<IAbpServiceConvention, TinyAbpServiceConvention>()
        );

        await base.ConfigureServicesAsync(context);
    }
}
