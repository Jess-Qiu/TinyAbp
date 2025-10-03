using FluentValidation;
using Volo.Abp.DependencyInjection;

namespace Services.Test.Dtos;

public class TestInputValidator : AbstractValidator<TestInput>, ITransientDependency
{
    public TestInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
