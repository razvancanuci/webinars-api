using Application.Requests;
using Asp.Versioning.Builder;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebAPI.Endpoints.Filters;
using DateTime = System.DateTime;

namespace WebAPI.Endpoints;

public static class WebinarEndpoints
{
    public static void AddWebinarEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        app.MapGet("/api/v{apiVersion:apiVersion}/webinar", async (
                [FromRoute] string apiVersion,
                [FromQuery] int page,
                ISender sender) =>
            {
                var availableWebinarsRequest = new AvailableWebinarsRequest { Page = page };
                
                var result = await sender.Send(availableWebinarsRequest);
                return result;
            })
            .AddEndpointFilter<AvailableWebinarsEndpointFilter>()
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1)
            .Produces<Ok>();

        app.MapGet("/api/v{apiVersion:apiVersion}/webinar/{id}", async (
                [FromRoute] string apiVersion,
                [FromRoute] string id,
                ISender sender) =>
            {
                var webinarByIdRequest = new AvailableWebinarByIdRequest(id);
                var result = await sender.Send(webinarByIdRequest);

                return result;
            })
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1)
            .Produces<Ok>()
            .Produces<NotFound>();

        app.MapPatch("/api/v{apiVersion:apiVersion}/webinar/{id}", async (
                [FromRoute] string apiVersion,
                [FromRoute] string id,
                [FromBody] Person personRequest,
                ISender sender) =>
            {
                var request = new RegisterWebinarRequest
                {
                    WebinarId = id,
                    Person = personRequest
                };
                
                var result = await sender.Send(request);

                return result;
            })
            .AddEndpointFilter<RegisterWebinarEndpointFilter>()
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1)
            .Produces<Ok>()
            .Produces<BadRequest>()
            .Produces<NotFound>();

        app.MapPost("/api/v{apiVersion:apiVersion}/webinar",  async(
                [FromRoute] string apiVersion,
                [FromForm] string title,
                [FromForm] string description,
                [FromForm] string host,
                [FromForm] DateTime dateScheduled,
                IFormFile image,
                ISender sender) =>
            {
                var request = new NewWebinarRequest()
                {
                    Title = title,
                    Host = host,
                    Description = description,
                    DateScheduled = dateScheduled,
                    Image = image
                };
                
                var result = await sender.Send(request);

                return result;
            })
            .AddEndpointFilter<NewWebinarEndpointFilter>()
            .DisableAntiforgery()
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1)
            .Produces<Created>()
            .Produces<BadRequest>();

        app.MapDelete("/api/v{apiVersion:apiVersion}/webinar/{id}", async (
                [FromRoute] string apiVersion,
                [FromRoute] string id,
                ISender sender) =>
            {
                var request = new CancelWebinarRequest(id);

                var result = await sender.Send(request);

                return result;
            })
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1)
            .Produces<NoContent>()
            .Produces<NotFound>();
    }
}