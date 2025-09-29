using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// API文档应用程序构建器扩展类
/// 提供API文档中间件的扩展方法
/// </summary>
public static class ApiDocumentApplicationBuilderExtension
{
    /// <summary>
    /// 使用API文档中间件
    /// 配置Swagger和Scalar API文档生成
    /// </summary>
    /// <param name="app">应用程序构建器</param>
    /// <param name="defaultApiDocumentName">默认API文档名称</param>
    /// <returns>应用程序构建器</returns>
    public static IApplicationBuilder UseApiDocument(
        this IApplicationBuilder app,
        string defaultApiDocumentName = "default"
    )
    {
        // 验证应用程序构建器参数不为空
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        // 从依赖注入容器中获取Swagger生成选项
        var swaggerGenOptions = app
            .ApplicationServices.GetRequiredService<IOptions<SwaggerGenOptions>>()
            .Value;

        // 配置端点路由
        app.UseEndpoints(config =>
        {
            // 映射Swagger端点，提供OpenAPI JSON文档
            config.MapSwagger("/openapi/{documentName}.json");
            // 映射Scalar API文档参考端点
            config.MapScalarApiReference(options => options.AddDocument(defaultApiDocumentName));
        });

        return app;
    }
}
