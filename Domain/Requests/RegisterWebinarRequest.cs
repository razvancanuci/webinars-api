using Domain.Entities;
using MediatR;

namespace Domain.Requests;

public class RegisterWebinarRequest : IRequest
{
    public int WebinarId { get; set; }
    public Person Person { get; set; } = null!;
}