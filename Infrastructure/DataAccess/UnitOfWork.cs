﻿using Domain.Interfaces;

namespace DataAccess;

public class UnitOfWork: IUnitOfWork
{
    public IWebinarRepository WebinarRepository { get; init; }

    private readonly WebinarContext _context;

    public UnitOfWork(IWebinarRepository webinarRepository, WebinarContext context)
    {
        WebinarRepository = webinarRepository;
        _context = context;
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}