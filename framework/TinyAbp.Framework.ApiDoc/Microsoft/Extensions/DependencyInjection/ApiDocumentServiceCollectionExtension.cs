using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApiDocumentServiceCollectionExtension
{
    public static IServiceCollection AddApiDocument(
        this IServiceCollection services,
        Action<SwaggerGenOptions>? action = null
    )
    {
        var mvcOptions = services.GetPreConfigureActions<AbpAspNetCoreMvcOptions>().Configure();

        var remoteServiceSettings =
            mvcOptions.ConventionalControllers.ConventionalControllerSettings.DistinctBy(x =>
                x.RemoteServiceName
            );
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            if (action == null)
            {
                options.SwaggerDoc(
                    "default",
                    new OpenApiInfo { Title = "TinyAbp Framework API Document", Version = "v1" }
                );
            }
            else
            {
                action?.Invoke(options);
            }

            ConfigurateApiDocGroups(options, remoteServiceSettings);
            ConfigurateApiDocFilter(options, remoteServiceSettings);
            ConfigurateLoadAssemblyXml(options);
        });

        return services;
    }

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

    private static void ConfigurateLoadAssemblyXml(SwaggerGenOptions options)
    {
        var basePath = AppContext.BaseDirectory;
        var xmlFiles = Directory.GetFiles(basePath, "*.xml");

        foreach (var xmlFile in xmlFiles)
        {
            options.IncludeXmlComments(xmlFile, true);
        }
    }
}
