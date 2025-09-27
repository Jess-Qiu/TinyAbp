using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Autofac Web应用程序构建器扩展类
/// </summary>
public static class AutofacWebApplicationBuilderExtension
{
    /// <summary>
    /// 注册Autofac容器到WebApplicationBuilder
    /// </summary>
    /// <param name="builder">Web应用程序构建器</param>
    /// <returns>WebApplicationBuilder实例</returns>
    /// <exception cref="ArgumentNullException">当builder为null时抛出</exception>
    public static WebApplicationBuilder RegisterAutofacContainer(this WebApplicationBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Host.UseAutofac();

        return builder;
    }
}
