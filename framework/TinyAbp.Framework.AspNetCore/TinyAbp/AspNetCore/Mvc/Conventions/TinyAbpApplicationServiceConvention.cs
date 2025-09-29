using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace TinyAbp.AspNetCore.Mvc.Conventions;

/// <summary>
/// TinyAbp服务约定
/// 自定义服务约定，处理路由规范化
/// </summary>
public class TinyAbpServiceConvention : AbpServiceConvention
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options">ASP.NET Core MVC选项</param>
    /// <param name="conventionalRouteBuilder">约定路由构建器</param>
    public TinyAbpServiceConvention(
        IOptions<AbpAspNetCoreMvcOptions> options,
        IConventionalRouteBuilder conventionalRouteBuilder
    )
        : base(options, conventionalRouteBuilder) { }

    /// <summary>
    /// 规范化选择器路由
    /// 重写路由模板，确保包含根路径
    /// </summary>
    /// <param name="rootPath">根路径</param>
    /// <param name="controllerName">控制器名称</param>
    /// <param name="action">操作模型</param>
    /// <param name="configuration">控制器设置</param>
    protected override void NormalizeSelectorRoutes(
        string rootPath,
        string controllerName,
        ActionModel action,
        ConventionalControllerSetting? configuration
    )
    {
        // 调用基类的路由规范化方法
        base.NormalizeSelectorRoutes(rootPath, controllerName, action, configuration);

        // 检查路由模板是否包含根路径
        foreach (var selector in action.Selectors)
        {
            if (selector.AttributeRouteModel != null)
            {
                var template = selector.AttributeRouteModel.Template;

                // 如果模板不为空且不以前斜杠开头，添加根路径前缀
                if (!template.IsNullOrWhiteSpace() && !template.StartsWith("/"))
                {
                    selector.AttributeRouteModel.Template = $"{rootPath}/{template}";
                }
            }
        }
    }
}
