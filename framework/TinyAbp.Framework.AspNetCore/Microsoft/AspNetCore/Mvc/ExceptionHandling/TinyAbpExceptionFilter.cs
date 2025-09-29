using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Http;

namespace Microsoft.AspNetCore.Mvc.ExceptionHandling;

/// <summary>
/// TinyAbp异常过滤器
/// 自定义异常处理逻辑，包装错误响应
/// </summary>
public class TinyAbpExceptionFilter : AbpExceptionFilter
{
    /// <summary>
    /// 处理和包装异常
    /// 重写基类方法，自定义错误响应格式
    /// </summary>
    /// <param name="context">异常上下文</param>
    protected override async Task HandleAndWrapException(ExceptionContext context)
    {
        // 调用基类的异常处理方法
        await base.HandleAndWrapException(context);

        // 检查结果是否为对象结果
        if (context.Result is ObjectResult objectResult)
        {
            // 如果结果是远程服务错误响应，则提取错误信息
            if (objectResult.Value is RemoteServiceErrorResponse errorInfoResult)
            {
                // 重新包装错误响应，只返回错误信息部分
                context.Result = new ObjectResult(errorInfoResult.Error);
            }
        }
    }
}
