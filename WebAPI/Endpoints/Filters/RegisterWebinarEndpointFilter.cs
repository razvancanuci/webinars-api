using Application.Requests;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Endpoints.Filters;

public class RegisterWebinarEndpointFilter(IValidator<RegisterWebinarRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var id = context.GetArgument<string>(1);
        var person = context.GetArgument<Person>(2);

        var request = new RegisterWebinarRequest
        {
            WebinarId = id,
            Person = person
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