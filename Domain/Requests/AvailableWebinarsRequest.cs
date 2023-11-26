using Domain.Entities;
using MediatR;

namespace Domain.Requests;

public class AvailableWebinarsRequest : IRequest<IQueryable<Webinar>>
{
    public int ItemsPerPage { get; } = 20;
    public int Page { get; init; }
}