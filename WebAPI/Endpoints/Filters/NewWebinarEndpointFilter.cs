using Application.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Endpoints.Filters;

public class NewWebinarEndpointFilter(IValidator<NewWebinarRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var title = context.GetArgument<string>(1);
        var description = context.GetArgument<string>(2);
        var host = context.GetArgument<string>(3);
        var dateScheduled = context.GetArgument<DateTime>(4);
        var image = context.GetArgument<IFormFile>(5);
        
        var request = new NewWebinarRequest
        {
            Title = title,
            Description = description,
            Host = host,
            DateScheduled = dateScheduled,
            Image = image
        };
        
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var result = await next(context);
        return result;
    }
}