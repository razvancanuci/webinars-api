using Asp.Versioning;
using Domain.Requests;
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
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAvailableWebinarByIdAsync(int id)
    {
        return Ok(id);
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RegisterToWebinarAsync(RegisterWebinarRequest request)
    {
        await _mediator.Send(request);
        return NoContent();
    }
}