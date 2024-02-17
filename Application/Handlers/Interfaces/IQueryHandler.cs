﻿using Application.Requests.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Handlers.Interfaces;

public interface IQueryHandler<T> : IRequestHandler<T, IResult>
    where T: IQueryRequest
{
    
}