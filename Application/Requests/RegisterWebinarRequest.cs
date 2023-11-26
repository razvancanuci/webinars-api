using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Requests;

public class RegisterWebinarRequest : IRequest<IActionResult>
{
    public int WebinarId { get; set; }
    public Person Person { get; set; } = null!;
}