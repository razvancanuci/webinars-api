using System.Diagnostics.CodeAnalysis;
using Application.Requests;
using Asp.Versioning.Builder;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Endpoints.Filters;
using DateTime = System.DateTime;

namespace WebAPI.Endpoints;

[ExcludeFromCodeCoverage]
public static class WebinarEndpoints
{
    public static void AddWebinarEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        app.MapGet("/api/v{apiVersion:apiVersion}/webinar", async (
                [FromRoute] string apiVersion,
                [FromQuery] int page,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var availableWebinarsRequest = new AvailableWebinarsRequest { Page = page };
                
                var result = await sender.Send(availableWebinarsRequest, cancellationToken);
                return result;
            })
            .AddEndpointFilter<AvailableWebinarsEndpointFilter>()
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1)
            .Produces<Ok>()
            .CacheOutput();

        app.MapGet("/api/v{apiVersion:apiVersion}/webinar/{id}", async (
                [FromRoute] string apiVersion,
                [FromRoute] string id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var webinarByIdRequest = new AvailableWebinarByIdRequest(id);
                var result = await sender.Send(webinarByIdRequest, cancellationToken);

                return result;
            })
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1)
            .Produces<Ok>()
            .Produces<NotFound>()
            .CacheOutput();
        
        app.MapGet("/api/v{apiVersion:apiVersion}/webinar/{id}/image", async (
                [FromRoute] string apiVersion,
                [FromRoute] string id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var webinarImageRequest = new DownloadWebinarImageRequest(id);
                var result = await sender.Send(webinarImageRequest, cancellationToken);

                return result;
            })
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1)
            .Produces<Ok>()
            .Produces<NotFound>()
            .CacheOutput();

        app.MapPatch("/api/v{apiVersion:apiVersion}/webinar/{id}", async (
                [FromRoute] string apiVersion,
                [FromRoute] string id,
                [FromBody] PersonDto personRequest,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var request = new RegisterWebinarRequest
                {
                    WebinarId = id,
                    Person = personRequest
                };
                
                var result = await sender.Send(request, cancellationToken);

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
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var request = new NewWebinarRequest()
                {
                    Title = title,
                    Host = host,
                    Description = description,
                    DateScheduled = dateScheduled,
                    Image = image
                };
                
                var result = await sender.Send(request, cancellationToken);

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
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var request = new CancelWebinarRequest(id);

                var result = await sender.Send(request, cancellationToken);

                return result;
            })
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1)
            .Produces<NoContent>()
            .Produces<NotFound>();
    }
}