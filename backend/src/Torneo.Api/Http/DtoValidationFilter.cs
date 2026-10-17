using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Torneo.Domain.Common;

namespace Torneo.Api.Http;

public sealed class DtoValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
            {
                continue;
            }

            var result = await validator.ValidateAsync(ContextFor(argument));
            if (result.IsValid)
            {
                continue;
            }

            context.Result = new BadRequestObjectResult(new ApiError(ErrorCodes.Validation, result.Errors[0].ErrorMessage));
            return;
        }

        await next();
    }

    private static IValidationContext ContextFor(object model)
    {
        var contextType = typeof(ValidationContext<>).MakeGenericType(model.GetType());
        return (IValidationContext)Activator.CreateInstance(contextType, model)!;
    }
}
