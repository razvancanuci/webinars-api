using Application.Requests;
using Asp.Versioning;
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
        var webinarByIdRequest = new AvailableWebinarByIdRequest { WebinarId = id };
        var result = await _mediator.Send(webinarByIdRequest);
        return Ok();
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RegisterToWebinarAsync(RegisterWebinarRequest request)
    {
        var result = await _mediator.Send(request);
        return result;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddWebinarAsync(NewWebinarRequest request)
    {
        var result = await _mediator.Send(request);

        return result;
    }
}