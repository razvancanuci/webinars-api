using Application.Requests;
using Asp.Versioning;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class WebinarController : ControllerBase
{
    private readonly IMediator _mediator;
    public WebinarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableWebinarsAsync(int page)
    {
        var availableWebinarsRequest = new AvailableWebinarsRequest { Page = page };
        var result = await _mediator.Send(availableWebinarsRequest);
        
        return result;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvailableWebinarByIdAsync(string id)
    {
        var webinarByIdRequest = new AvailableWebinarByIdRequest(id);
        var result = await _mediator.Send(webinarByIdRequest);
        
        return result;
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterToWebinarAsync(string id, Person personRequest)
    {
        var request = new RegisterWebinarRequest
        {
            WebinarId = id,
            Person = personRequest
        };
        
        var result = await _mediator.Send(request);
        
        return result;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddWebinarAsync(NewWebinarRequest request)
    {
        var result = await _mediator.Send(request);
        
        return result;
    }
}