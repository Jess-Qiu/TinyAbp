using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// API文档服务集合扩展类
/// 提供Swagger API文档生成的扩展方法
/// </summary>
public static class ApiDocumentServiceCollectionExtension
{
    /// <summary>
    /// 添加API文档服务
    /// 配置Swagger生成器和相关服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="action">Swagger配置操作</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddApiDocument(
        this IServiceCollection services,
        Action<SwaggerGenOptions>? action = null
    )
    {
        // 获取预配置的MVC选项
        var mvcOptions = services.GetPreConfigureActions<AbpAspNetCoreMvcOptions>().Configure();

        // 获取远程服务设置，按服务名称去重
        var remoteServiceSettings =
            mvcOptions.ConventionalControllers.ConventionalControllerSettings.DistinctBy(x =>
                x.RemoteServiceName
            );

        // 添加API端点资源管理器
        services.AddEndpointsApiExplorer();

        // 配置Swagger生成器
        services.AddSwaggerGen(options =>
        {
            // 如果没有提供自定义配置，使用默认配置
            if (action == null)
            {
                options.SwaggerDoc(
                    "default",
                    new OpenApiInfo { Title = "TinyAbp Framework API Document", Version = "v1" }
                );
            }
            else
            {
                // 调用自定义配置操作
                action?.Invoke(options);
            }

            // 配置API文档分组
            ConfigurateApiDocGroups(options, remoteServiceSettings);
            // 配置API文档过滤器
            ConfigurateApiDocFilter(options, remoteServiceSettings);
            // 配置加载程序集XML注释文件
            ConfigurateLoadAssemblyXml(options);
        });

        return services;
    }

    /// <summary>
    /// 配置API文档分组
    /// 为每个远程服务创建对应的Swagger文档
    /// </summary>
    /// <param name="options">Swagger生成器选项</param>
    /// <param name="remoteServiceSettings">远程服务设置集合</param>
    private static void ConfigurateApiDocGroups(
        SwaggerGenOptions options,
        IEnumerable<ConventionalControllerSetting> remoteServiceSettings
    )
    {
        if (remoteServiceSettings.Any())
        {
            foreach (var setting in remoteServiceSettings)
            {
                if (
                    !options.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey(
                        setting.RemoteServiceName
                    )
                )
                {
                    options.SwaggerDoc(
                        setting.RemoteServiceName,
                        new() { Title = setting.RemoteServiceName, Version = "v1" }
                    );
                }
            }
        }
    }

    /// <summary>
    /// 配置API文档过滤器
    /// 根据控制器所属的程序集匹配对应的服务文档
    /// </summary>
    /// <param name="options">Swagger生成器选项</param>
    /// <param name="remoteServiceSettings">远程服务设置集合</param>
    private static void ConfigurateApiDocFilter(
        SwaggerGenOptions options,
        IEnumerable<ConventionalControllerSetting> remoteServiceSettings
    )
    {
        if (remoteServiceSettings.Any())
        {
            options.DocInclusionPredicate(
                (docName, apiDesc) =>
                {
                    if (
                        apiDesc.ActionDescriptor
                        is ControllerActionDescriptor controllerActionDescriptor
                    )
                    {
                        var matchingSetting = remoteServiceSettings.FirstOrDefault(setting =>
                            setting.Assembly
                            == controllerActionDescriptor.ControllerTypeInfo.Assembly
                        );

                        return matchingSetting?.RemoteServiceName == docName;
                    }

                    return false;
                }
            );
        }
    }

    /// <summary>
    /// 配置加载程序集XML注释文件
    /// 自动加载应用程序目录下的所有XML注释文件
    /// </summary>
    /// <param name="options">Swagger生成器选项</param>
    private static void ConfigurateLoadAssemblyXml(SwaggerGenOptions options)
    {
        // 获取应用程序基目录
        var basePath = AppContext.BaseDirectory;
        // 查找所有XML注释文件
        var xmlFiles = Directory.GetFiles(basePath, "*.xml");

        // 为每个XML文件添加注释到Swagger
        foreach (var xmlFile in xmlFiles)
        {
            options.IncludeXmlComments(xmlFile, true);
        }
    }
}
