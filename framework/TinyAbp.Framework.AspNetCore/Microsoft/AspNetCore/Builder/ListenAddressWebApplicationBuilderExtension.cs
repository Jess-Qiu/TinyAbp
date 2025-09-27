using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 监听地址Web应用程序构建器扩展类
/// </summary>
public static class ListenAddressWebApplicationBuilderExtension
{
    // 默认配置节名称
    private const string DefaultSection = "App:ListenAddress";

    /// <summary>
    /// 注册监听地址到WebApplicationBuilder
    /// </summary>
    /// <param name="builder">Web应用程序构建器</param>
    /// <returns>WebApplicationBuilder实例</returns>
    public static WebApplicationBuilder RegisterListenAddress(this WebApplicationBuilder builder)
    {
        // 获取监听地址配置节
        var section = builder.Configuration.GetSection(DefaultSection);

        // 设置默认监听地址
        var listenAddress = new string[] { "http://*:5000", "https://*:5001" };

        // 如果配置节存在，则使用配置的地址
        if (section.Exists())
        {
            listenAddress = section.Get<string[]>();
        }

        // 如果有监听地址，则应用到WebHost
        if (listenAddress?.Any() != true)
            throw new InvalidOperationException("请配置服务的监听地址");

        builder.WebHost.UseUrls(listenAddress);

        foreach (var uri in listenAddress)
        {
            Log.Warning("服务监听地址: {ListenAddress}", uri);
        }

        return builder;
    }
}
