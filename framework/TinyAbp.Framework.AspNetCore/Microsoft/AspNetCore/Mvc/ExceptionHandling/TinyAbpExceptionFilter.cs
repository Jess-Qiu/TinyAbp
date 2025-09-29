using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Http;

namespace Microsoft.AspNetCore.Mvc.ExceptionHandling;

public class TinyAbpExceptionFilter : AbpExceptionFilter
{
    protected override async Task HandleAndWrapException(ExceptionContext context)
    {
        await base.HandleAndWrapException(context);

        if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.Value is RemoteServiceErrorResponse errorInfoResult)
            {
                context.Result = new ObjectResult(errorInfoResult.Error);
            }
        }
    }
}
