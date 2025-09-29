using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace TinyAbp.AspNetCore.Mvc.Conventions;

public class TinyAbpConventionalRouteBuilder : ConventionalRouteBuilder
{
    public TinyAbpConventionalRouteBuilder(IOptions<AbpConventionalControllerOptions> options)
        : base(options) { }

    public override string Build(
        string rootPath,
        string controllerName,
        ActionModel action,
        string httpMethod,
        ConventionalControllerSetting? configuration
    )
    {
        return base.Build(rootPath, controllerName, action, httpMethod, configuration);
    }

    protected override string GetApiRoutePrefix(
        ActionModel actionModel,
        ConventionalControllerSetting? configuration
    ) => string.Empty;
}
