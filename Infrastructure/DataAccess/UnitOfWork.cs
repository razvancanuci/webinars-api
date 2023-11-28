using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess;

public class UnitOfWork: IUnitOfWork
{
    public IRepository<Webinar> WebinarRepository { get; init; }

    private readonly WebinarContext _context;

    public UnitOfWork(IRepository<Webinar> webinarRepository, WebinarContext context)
    {
        WebinarRepository = webinarRepository;
        _context = context;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}