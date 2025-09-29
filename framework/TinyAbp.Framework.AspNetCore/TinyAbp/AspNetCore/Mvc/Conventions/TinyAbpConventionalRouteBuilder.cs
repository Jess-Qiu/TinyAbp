using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace TinyAbp.AspNetCore.Mvc.Conventions;

/// <summary>
/// TinyAbp约定路由构建器
/// 自定义路由构建逻辑，重写API路由前缀处理
/// </summary>
public class TinyAbpConventionalRouteBuilder : ConventionalRouteBuilder
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="options">约定控制器选项</param>
    public TinyAbpConventionalRouteBuilder(IOptions<AbpConventionalControllerOptions> options)
        : base(options) { }

    /// <summary>
    /// 构建路由
    /// </summary>
    /// <param name="rootPath">根路径</param>
    /// <param name="controllerName">控制器名称</param>
    /// <param name="action">操作模型</param>
    /// <param name="httpMethod">HTTP方法</param>
    /// <param name="configuration">控制器设置</param>
    /// <returns>构建的路由字符串</returns>
    public override string Build(
        string rootPath,
        string controllerName,
        ActionModel action,
        string httpMethod,
        ConventionalControllerSetting? configuration
    )
    {
        // 调用基类的构建方法
        return base.Build(rootPath, controllerName, action, httpMethod, configuration);
    }

    /// <summary>
    /// 获取API路由前缀
    /// 重写此方法以返回空字符串，移除默认的API前缀
    /// </summary>
    /// <param name="actionModel">操作模型</param>
    /// <param name="configuration">控制器设置</param>
    /// <returns>空字符串，表示不使用API路由前缀</returns>
    protected override string GetApiRoutePrefix(
        ActionModel actionModel,
        ConventionalControllerSetting? configuration
    ) => string.Empty;
}
