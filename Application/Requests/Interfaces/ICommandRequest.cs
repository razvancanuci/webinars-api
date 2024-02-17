using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Requests.Interfaces;

public interface ICommandRequest : IRequest<IResult>
{
}