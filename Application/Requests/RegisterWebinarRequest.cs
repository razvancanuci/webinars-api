using Application.Requests.Interfaces;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Requests;
#nullable disable
public class RegisterWebinarRequest : ICommandRequest
{
    public string WebinarId { get; set; }
    public PersonDto Person { get; set; } = null!;
}