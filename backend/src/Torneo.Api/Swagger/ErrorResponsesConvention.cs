using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Torneo.Api.Http;

namespace Torneo.Api.Swagger;

public sealed class ErrorResponsesConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var action in application.Controllers.SelectMany(controller => controller.Actions))
        {
            action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiError), StatusCodes.Status400BadRequest));
            action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiError), StatusCodes.Status404NotFound));
            action.Filters.Add(new ProducesResponseTypeAttribute(typeof(ApiError), StatusCodes.Status409Conflict));
        }
    }
}
