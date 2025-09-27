using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Serilog Web应用程序构建器扩展类
/// </summary>
public static class SerilogWebApplicationBuilderExtension
{
    // 默认Serilog配置节名称
    private const string DefaultSectionName = "Serilog";

    /// <summary>
    /// 注册Serilog日志记录器到WebApplicationBuilder
    /// </summary>
    /// <param name="builder">Web应用程序构建器</param>
    /// <returns>WebApplicationBuilder实例</returns>
    public static WebApplicationBuilder RegisterSerilogLogger(this WebApplicationBuilder builder)
    {
        // 使用Serilog作为日志提供程序
        builder.Host.UseSerilog();

        // 创建全局日志记录器
        CreateGlobalLogger(builder);

        return builder;
    }

    /// <summary>
    /// 创建全局Serilog日志记录器配置
    /// </summary>
    /// <param name="builder">Web应用程序构建器</param>
    /// <param name="logEventLevel">日志事件级别，默认为Information</param>
    private static void CreateGlobalLogger(
        WebApplicationBuilder builder,
        LogEventLevel logEventLevel = LogEventLevel.Information
    )
    {
        // 获取Serilog配置节
        var section = builder.Configuration.GetSection(DefaultSectionName);

        // 创建日志配置实例
        var loggerConfiguration = new LoggerConfiguration();

        // 如果配置节存在，则从配置读取日志设置
        if (section.Exists())
        {
            loggerConfiguration.ReadFrom.Configuration(builder.Configuration);
        }
        else
        {
            // 使用默认日志配置
            loggerConfiguration = loggerConfiguration
                // 排除任务取消异常日志
                .Filter.ByExcluding(log =>
                    log.Exception?.GetType() == typeof(TaskCanceledException)
                    || log.MessageTemplate.Text.Contains("\"message\": \"A task was canceled.\"")
                )
                // 设置基础日志级别
                .MinimumLevel.Is(logEventLevel)
                // 覆盖Microsoft命名空间日志级别
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                // 覆盖ASP.NET Core诊断日志级别
                .MinimumLevel.Override(
                    "Microsoft.AspNetCore.Hosting.Diagnostics",
                    LogEventLevel.Error
                )
                // 添加上下文信息
                .Enrich.FromLogContext()
                // 配置异步文件日志输出（所有日志）
                .WriteTo.Async(c =>
                    c.File(
                        formatter: new CompactJsonFormatter(),
                        "logs/all/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: LogEventLevel.Debug
                    )
                )
                // 配置异步文件日志输出（错误日志）
                .WriteTo.Async(c =>
                    c.File(
                        formatter: new CompactJsonFormatter(),
                        "logs/error/errorlog-.txt",
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: LogEventLevel.Error
                    )
                )
                // 配置异步控制台日志输出
                .WriteTo.Async(c =>
                    c.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                );
        }

        // 设置全局日志记录器实例
        Log.Logger = loggerConfiguration.CreateLogger();
    }
}
