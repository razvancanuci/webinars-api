using Application.Requests;
using Asp.Versioning;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Obsolete]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class WebinarController : ControllerBase
{
    private readonly ISender _sender;
    public WebinarController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> GetAvailableWebinarsAsync(int page)
    {
        var availableWebinarsRequest = new AvailableWebinarsRequest { Page = page };
        var result = await _sender.Send(availableWebinarsRequest);
        
        return result;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetAvailableWebinarByIdAsync(string id)
    {
        var webinarByIdRequest = new AvailableWebinarByIdRequest(id);
        var result = await _sender.Send(webinarByIdRequest);
        
        return result;
    }
    
    [HttpGet("{id}/image")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> DownloadImageAsync(string id)
    {
        var webinarByIdRequest = new DownloadWebinarImageRequest(id);
        var result = await _sender.Send(webinarByIdRequest);
        
        return result;
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> RegisterToWebinarAsync(string id, Person personRequest)
    {
        var request = new RegisterWebinarRequest
        {
            WebinarId = id,
            Person = personRequest
        };
        
        var result = await _sender.Send(request);
        
        return result;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> AddWebinarAsync([FromForm] NewWebinarRequest request)
    {
        var result = await _sender.Send(request);
        
        return result;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> CancelWebinarAsync(string id)
    {
        var request = new CancelWebinarRequest(id);

        var result = await _sender.Send(request);

        return result;
    }
}