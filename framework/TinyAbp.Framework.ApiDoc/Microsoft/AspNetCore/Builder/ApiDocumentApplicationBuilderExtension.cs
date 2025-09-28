using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Builder;

public static class ApiDocumentApplicationBuilderExtension
{
    public static IApplicationBuilder UseApiDocument(
        this IApplicationBuilder app,
        string defaultApiDocumentName = "default"
    )
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        var swaggerGenOptions = app
            .ApplicationServices.GetRequiredService<IOptions<SwaggerGenOptions>>()
            .Value;

        app.UseEndpoints(config =>
        {
            config.MapSwagger("/openapi/{documentName}.json");
            config.MapScalarApiReference(options => options.AddDocument(defaultApiDocumentName));
        });

        return app;
    }
}
