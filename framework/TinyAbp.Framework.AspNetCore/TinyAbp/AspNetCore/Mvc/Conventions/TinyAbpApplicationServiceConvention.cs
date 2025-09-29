using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace TinyAbp.AspNetCore.Mvc.Conventions;

public class TinyAbpServiceConvention : AbpServiceConvention
{
    public TinyAbpServiceConvention(
        IOptions<AbpAspNetCoreMvcOptions> options,
        IConventionalRouteBuilder conventionalRouteBuilder
    )
        : base(options, conventionalRouteBuilder) { }

    protected override void NormalizeSelectorRoutes(
        string rootPath,
        string controllerName,
        ActionModel action,
        ConventionalControllerSetting? configuration
    )
    {
        base.NormalizeSelectorRoutes(rootPath, controllerName, action, configuration);

        // 检查路由模板是否包含根路径
        foreach (var selector in action.Selectors)
        {
            if (selector.AttributeRouteModel != null)
            {
                var template = selector.AttributeRouteModel.Template;

                if (!template.IsNullOrWhiteSpace() && !template.StartsWith("/"))
                {
                    selector.AttributeRouteModel.Template = $"{rootPath}/{template}";
                }
            }
        }
    }
}
