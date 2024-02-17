using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Requests.Interfaces;

public interface IQueryRequest : IRequest<IResult>
{
}