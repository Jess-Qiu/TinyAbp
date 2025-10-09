using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.DependencyInjection;

namespace TinyAbp.AspNetCore.Mvc.Validation;

/// <summary>
/// TinyAbp模型状态Fluent验证器
/// 使用FluentValidation对模型状态进行验证
/// </summary>
public class TinyAbpModelStateFluentValidator : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModelStateValidator _modelStateValidator;

    /// <summary>
    /// 构造函数 - 注入依赖服务
    /// </summary>
    /// <param name="serviceProvider">服务提供程序</param>
    /// <param name="modelStateValidator">模型状态验证器</param>
    public TinyAbpModelStateFluentValidator(
        IServiceProvider serviceProvider,
        IModelStateValidator modelStateValidator
    )
    {
        _serviceProvider = serviceProvider;
        _modelStateValidator = modelStateValidator;
    }

    /// <summary>
    /// 异步验证模型状态
    /// </summary>
    /// <param name="context">操作执行上下文</param>
    /// <returns>异步任务</returns>
    public async Task ValidateAsync(ActionExecutingContext context)
    {
        // 获取所有 action 参数
        var parameters = context.ActionArguments.Values.ToList();

        foreach (var parameter in parameters)
        {
            if (parameter == null)
                continue;

            // 获取该参数类型对应的验证器
            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.GetType());
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator != null)
            {
                // 执行验证
                var validationResult = await validator.ValidateAsync(
                    new ValidationContext<object>(parameter)
                );

                // 如果验证失败，设置模型状态错误
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }

                    _modelStateValidator.Validate(context.ModelState);
                    return;
                }
            }
        }
    }
}
