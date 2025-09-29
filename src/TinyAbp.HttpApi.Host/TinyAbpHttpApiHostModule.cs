using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ExceptionHandling;
using TinyAbp.Application;
using TinyAbp.Framework.AspNetCore;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.Json;
using Volo.Abp.Auditing;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace TinyAbp.HttpApi.Host;

/// <summary>
/// Tiny Abp HttpApi Host Module - 主应用程序模块配置
/// </summary>
[DependsOn(typeof(TinyAbpFrameworkAspNetCoreModule), typeof(TinyAbpApplicationModule))]
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
        PreConfigure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(
                typeof(TinyAbpApplicationModule).Assembly,
                opt => opt.RemoteServiceName = "default"
            );

            foreach (var setting in options.ConventionalControllers.ConventionalControllerSettings)
            {
                setting.RootPath = "api";
            }
        });

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

        // 配置 Mvc 过滤器
        ConfigureMvcFilter(context);

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
    private void ConfigureCors(ServiceConfigurationContext context)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                var origins =
                    context
                        .Services.GetConfiguration()
                        .GetSection("App:CorsOrigins")
                        .Get<string[]>()
                    ?? Array.Empty<string>();

                builder.WithOrigins(origins);
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
            });
        });
    }

    /// <summary>
    /// 配置JSON序列化选项
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureJsonOptions(ServiceConfigurationContext context)
    {
        Configure<AbpJsonOptions>(options =>
        {
            options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        });

        context.Services.AddMvcCore().AddAbpJson();
    }

    /// <summary>
    /// 配置异常处理
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureException(ServiceConfigurationContext context)
    {
        Configure<AbpExceptionHandlingOptions>(options =>
        {
            options.SendExceptionsDetailsToClients = true;
        });
    }

    /// <summary>
    /// 配置审计日志
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureAuditing(ServiceConfigurationContext context)
    {
        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabled = true;
            options.ApplicationName = "TinyAbp";
        });
    }

    private void ConfigureMvcFilter(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<TinyAbpExceptionFilter>();

        context.Services.AddMvc(options =>
        {
            options.Filters.RemoveAll(x =>
                (x as ServiceFilterAttribute)?.ServiceType == typeof(AbpExceptionFilter)
            );
            options.Filters.AddService<TinyAbpExceptionFilter>();
        });
    }

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
        var env = context.GetEnvironment();

        app.UseCors();
        app.UseRouting();
        app.UseApiDocument();
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        await base.OnApplicationInitializationAsync(context);
    }
}
