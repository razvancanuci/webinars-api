using Application.Requests.Interfaces;
using Domain.Entities;

namespace Application.Requests;
#nullable disable
public class RegisterWebinarRequest : ICommandRequest
{
    public string WebinarId { get; set; }
    public Person Person { get; set; } = null!;
}