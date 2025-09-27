using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TinyAbp.Framework.AspNetCore.Microsoft.AspNetCore.Builder;

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
        if (listenAddress != null && listenAddress.Any())
        {
            builder.WebHost.UseUrls(listenAddress);
        }

        return builder;
    }
}
