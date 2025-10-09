using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TinyAbp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

/// <summary>
/// TinyAbp Framework AspNetCore 模块
/// 集成ASP.NET Core相关功能，提供MVC、验证、异常处理等核心功能
/// </summary>
[DependsOn(typeof(AbpAspNetCoreMvcModule), typeof(AbpAspNetCoreSerilogModule))]
public class TinyAbpFrameworkAspNetCoreModule : AbpModule
{
    /// <summary>
    /// 配置服务 - 注册ASP.NET Core相关服务
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns>异步任务</returns>
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
