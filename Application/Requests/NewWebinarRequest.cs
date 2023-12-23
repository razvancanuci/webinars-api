using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Requests;

#nullable disable
public class NewWebinarRequest : IRequest<IActionResult>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Host { get; set; }
    public DateTime DateScheduled { get; set; }
    public IFormFile Image { get; set; }
}