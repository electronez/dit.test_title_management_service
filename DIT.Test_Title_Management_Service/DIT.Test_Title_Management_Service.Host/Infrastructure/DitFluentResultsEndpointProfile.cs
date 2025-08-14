namespace DIT.Test_Title_Management_Service.Host.Infrastructure;

using DIT.Test_Title_Management_Service.Application.Results;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

public class DitFluentResultsEndpointProfile : DefaultAspNetCoreResultEndpointProfile
{
    public override ActionResult TransformFailedResultToActionResult(FailedResultToActionResultTransformationContext context)
    {
        var result = context.Result;

        if (result.HasError<NotFoundError>())
        {
            var error = result.Errors.First();
            return new NotFoundObjectResult(
                new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = error.Message,
                });
        }

        var errorDtos = result.Errors.Select(e => new ErrorDto
        {
            Message = e.Message,
        });

        return new BadRequestObjectResult(errorDtos);
    }
}