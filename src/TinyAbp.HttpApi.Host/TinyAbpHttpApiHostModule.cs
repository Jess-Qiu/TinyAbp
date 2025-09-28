using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace TinyAbp.HttpApi.Host;

/// <summary>
/// Tiny Abp HttpApi Host Module - 主应用程序模块配置
/// </summary>
[DependsOn(
    // 依赖Autofac依赖注入容器模块
    typeof(AbpAutofacModule),
    // 依赖ASP.NET Core MVC模块
    typeof(AbpAspNetCoreMvcModule),
    // 依赖ASP.NET Core Serilog日志模块
    typeof(AbpAspNetCoreSerilogModule)
)]
public class TinyAbpHttpApiHostModule : AbpModule
{
    /// <summary>
    /// 预配置服务 - 在ConfigureServices之前执行
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task PreConfigureServicesAsync(ServiceConfigurationContext context)
    {
        // 预配置阶段：在主要服务配置之前执行
        // todo 1: 配置模块
        await base.PreConfigureServicesAsync(context);
    }

    /// <summary>
    /// 配置服务 - 注册应用程序依赖项
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    /// <returns></returns>
    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        // 配置Swagger API文档
        ConfigureSwagger(context);

        // 配置跨域请求策略
        ConfigureCors(context);

        // 配置JSON序列化选项
        ConfigureJsonOptions(context);

        // 配置异常处理机制
        ConfigureException(context);

        // 配置审计日志功能
        ConfigureAuditing(context);

        await base.ConfigureServicesAsync(context);
    }

    /// <summary>
    /// 配置Swagger文档生成
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureSwagger(ServiceConfigurationContext context)
    {
        context.Services.AddApiDocument();
    }

    /// <summary>
    /// 配置跨域资源共享(CORS)
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureCors(ServiceConfigurationContext context) { }

    /// <summary>
    /// 配置JSON序列化选项
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureJsonOptions(ServiceConfigurationContext context) { }

    /// <summary>
    /// 配置异常处理
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureException(ServiceConfigurationContext context) { }

    /// <summary>
    /// 配置审计日志
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureAuditing(ServiceConfigurationContext context) { }

    /// <summary>
    /// 应用程序初始化 - 配置中间件管道
    /// </summary>
    /// <param name="context">应用程序初始化上下文</param>
    /// <returns></returns>
    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context
    )
    {
        var app = context.GetApplicationBuilder();
        app.UseRouting();
        app.UseApiDocument();

        await base.OnApplicationInitializationAsync(context);
    }
}
