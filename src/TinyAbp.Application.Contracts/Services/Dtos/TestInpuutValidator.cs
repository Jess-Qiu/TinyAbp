using FluentValidation;
using Volo.Abp.DependencyInjection;

namespace Services.Test.Dtos;

/// <summary>
/// 测试输入验证器
/// 使用FluentValidation对TestInput进行参数验证
/// </summary>
public class TestInputValidator : AbstractValidator<TestInput>, ITransientDependency
{
    /// <summary>
    /// 构造函数 - 配置验证规则
    /// </summary>
    public TestInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
