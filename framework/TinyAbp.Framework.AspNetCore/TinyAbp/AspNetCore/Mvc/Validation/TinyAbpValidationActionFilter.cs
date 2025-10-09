using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Filters;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;
using Volo.Abp.Validation;

namespace TinyAbp.AspNetCore.Mvc.Validation;

/// <summary>
/// TinyAbp验证操作过滤器
/// 在控制器操作执行前进行模型验证
/// </summary>
public class TinyAbpValidationActionFilter : IAsyncActionFilter, IAbpFilter, ITransientDependency
{
    /// <summary>
    /// 异步操作执行方法
    /// </summary>
    /// <param name="context">操作执行上下文</param>
    /// <param name="next">操作执行委托</param>
    /// <returns>异步任务</returns>
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        if (
            !context.ActionDescriptor.IsControllerAction()
            || !context.ActionDescriptor.HasObjectResult()
        )
        {
            await next();
            return;
        }

        if (
            !context
                .GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>()
                .Value.AutoModelValidation
        )
        {
            await next();
            return;
        }

        if (
            ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(
                context.ActionDescriptor.GetMethodInfo()
            ) != null
        )
        {
            await next();
            return;
        }

        if (
            ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(
                context.Controller.GetType()
            ) != null
        )
        {
            await next();
            return;
        }

        if (context.ActionDescriptor.GetMethodInfo().DeclaringType != context.Controller.GetType())
        {
            var baseMethod = context.ActionDescriptor.GetMethodInfo();

            var overrideMethod = context
                .Controller.GetType()
                .GetMethods()
                .FirstOrDefault(x =>
                    x.DeclaringType == context.Controller.GetType()
                    && x.Name == baseMethod.Name
                    && x.ReturnType == baseMethod.ReturnType
                    && x.GetParameters()
                        .Select(p => p.ToString())
                        .SequenceEqual(baseMethod.GetParameters().Select(p => p.ToString()))
                );

            if (overrideMethod != null)
            {
                if (
                    ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(
                        overrideMethod
                    ) != null
                )
                {
                    await next();
                    return;
                }
            }
        }

        await context.GetRequiredService<TinyAbpModelStateFluentValidator>().ValidateAsync(context);

        await next();
    }
}
