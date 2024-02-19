using Application.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Endpoints.Filters;

public class AvailableWebinarsEndpointFilter(IValidator<AvailableWebinarsRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var page = context.GetArgument<int>(1);

        var request = new AvailableWebinarsRequest { Page = page };

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var result = await next(context);
        return result;
    }
}