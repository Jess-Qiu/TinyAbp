using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace TinyAbp.Framework.ApiDoc;

[DependsOn(typeof(AbpSwashbuckleModule))]
public class TinyAbpFrameworkApiDocModule : AbpModule
{
    private const string DefaultDocumentName = "TinyAbp";
    private const string DefaultSectionName = "App:ApiDoc";

    public override async Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var services = context.Services;

        // 获取MVC配置选项
        var mvcOptions = services.GetPreConfigureActions<AbpAspNetCoreMvcOptions>().Configure();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            ConfigureSwaggerOptions(context, option);
        });

        await base.ConfigureServicesAsync(context);
    }

    private void ConfigureSwaggerOptions(
        ServiceConfigurationContext context,
        SwaggerGenOptions option
    )
    { // 获取 API 配置
        var section = context.Configuration.GetSection(DefaultSectionName);
        var title = "TinyAbp API Document";
        var version = "v1";
        var name = DefaultDocumentName;

        if (section.Exists())
        {
            title = section["Title"].IsNullOrWhiteSpace() ? title : section["Title"];
            version = section["Version"].IsNullOrWhiteSpace() ? title : section["Version"];
            name = section["Name"].IsNullOrWhiteSpace() ? title : section["Name"];
        }

        option.SwaggerDoc(name, new OpenApiInfo { Title = title, Version = version });
    }

    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context
    )
    {
        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder() as WebApplication;

        if (env.IsDevelopment())
        {
            app.MapSwagger("/openapi/{documentName}.json");
            app.MapScalarApiReference(options => options.AddDocument(DefaultDocumentName));
        }

        await base.OnApplicationInitializationAsync(context);
    }
}
