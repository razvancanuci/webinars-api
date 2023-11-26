using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Requests;
#nullable disable
public class RegisterWebinarRequest : IRequest<IActionResult>
{
    public string WebinarId { get; set; }
    public Person Person { get; set; } = null!;
}