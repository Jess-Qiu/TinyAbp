using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Json;
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
        Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        Configure<AbpJsonOptions>(options =>
        {
            options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        });
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
            options.SendStackTraceToClients = false;
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

        if (env.IsDevelopment())
        {
            app.UseApiDocument();
        }
        else
        {
            app.UseCors();
        }

        app.UseRouting();
        app.UseAuditing();
        app.UseConfiguredEndpoints();

        await base.OnApplicationInitializationAsync(context);
    }
}
