﻿using Application.Requests;
using Domain.Constants;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers.Queries;

public class GetAvailableWebinarsQueryHandler : RequestHandlerBase, IRequestHandler<AvailableWebinarsRequest, IActionResult>
{
    public GetAvailableWebinarsQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    public async Task<IActionResult> Handle(AvailableWebinarsRequest request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.WebinarRepository
            .GetAsync(entity => entity.ScheduleDate > DateTimeConstants.AvailabilityDate,
                query => query.OrderBy(x => x.ScheduleDate)
                    .Skip((request.Page - 1) * request.ItemsPerPage)
                    .Take(request.ItemsPerPage)
                    .Select(w => new Webinar
                    {
                        Id = w.Id,
                        Title = w.Title,
                        Host = w.Host
                    }),
                asNoTracking: true);
        var mappedResult = result.Adapt<IEnumerable<WebinarShortInfoDto>>();
        
        return  new OkObjectResult(mappedResult);
    }
}