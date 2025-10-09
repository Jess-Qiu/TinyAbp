using System.Reflection;
using FluentValidation;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using TinyAbp.Application;
using TinyAbp.AspNetCore.Mvc.ExceptionHandling;
using TinyAbp.AspNetCore.Mvc.Validation;
using TinyAbp.Framework.Caching.FreeRedis;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.Json;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace TinyAbp.HttpApi.Host;

/// <summary>
/// Tiny Abp HttpApi Host Module - 主应用程序模块配置
/// </summary>
[DependsOn(
    // Tiny Abp Module
    typeof(TinyAbpFrameworkAspNetCoreModule),
    typeof(TinyAbpApplicationModule),
    typeof(TinyAbpFrameworkCachingFreeRedisModule),
    // Abp Module
    typeof(AbpAutofacModule),
    typeof(AbpDistributedLockingModule)
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

        // 配置 Cache
        ConfigureCache(context);

        // 配置分布式锁
        ConfigureDistributedLocking(context);

        // 配置 Fluent Validator
        ConfigureFluentValidator(context);

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

    /// <summary>
    /// 配置Mvc过滤器
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureMvcFilter(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<TinyAbpExceptionFilter>();
        context.Services.AddTransient<TinyAbpValidationActionFilter>();

        Configure<MvcOptions>(options =>
        {
            // 添加异常过滤器
            options.Filters.RemoveAll(x =>
                (x as ServiceFilterAttribute)?.ServiceType == typeof(AbpExceptionFilter)
            );
            options.Filters.AddService<TinyAbpExceptionFilter>();

            // 添加验证过滤器
            options.Filters.RemoveAll(x =>
                (x as ServiceFilterAttribute)?.ServiceType == typeof(AbpValidationActionFilter)
            );
            options.Filters.AddService<TinyAbpValidationActionFilter>();
        });
    }

    /// <summary>
    /// 配置缓存
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureCache(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "TinyAbp_";
        });
    }

    /// <summary>
    /// 配置分布式锁
    /// </summary>
    /// <param name="context">服务配置上下文</param>
    private void ConfigureDistributedLocking(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var redisConfiguration = configuration["Redis:Configuration"];
        var redisEnabled = configuration["Redis:IsEnabled"];
        if (string.IsNullOrEmpty(redisEnabled) || bool.Parse(redisEnabled))
        {
            if (!redisConfiguration.IsNullOrWhiteSpace())
            {
                context.Services.AddSingleton<IDistributedLockProvider>(sp =>
                {
                    var connection = ConnectionMultiplexer.Connect(redisConfiguration);

                    return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
                });
            }
        }
    }

    /// <summary>
    /// 配置 FluentValidator
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ConfigureFluentValidator(ServiceConfigurationContext context)
    {
        context.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
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
