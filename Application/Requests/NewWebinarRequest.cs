using Application.Requests.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Requests;

#nullable disable
public class NewWebinarRequest : ICommandRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Host { get; set; }
    public DateTime DateScheduled { get; set; }
    public IFormFile Image { get; set; }
}