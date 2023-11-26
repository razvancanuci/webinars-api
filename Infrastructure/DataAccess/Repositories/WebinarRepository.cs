using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class WebinarRepository : IRepository
{
    private readonly WebinarContext _context;
    public WebinarRepository(WebinarContext context)
    {
        context.Database.EnsureCreated();
        _context = context;
    }
    public async Task<IEnumerable<Webinar>> GetWebinarsAsync(int page, int itemsPerPage)
    {
        var webinars = await _context.Webinars
            .Skip( (page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .AsNoTracking()
            .ToListAsync();
        
        return webinars;
    }

    public async Task<Webinar?> GetWebinarByIdAsync(string id)
    {
        var webinar = await _context.Webinars.FirstOrDefaultAsync(x => x.Id == id);
        return webinar;
    }

    public async Task RegisterPersonToWebinarAsync(Webinar webinar)
    {
        await SaveAsync();
    }

    public async Task AddWebinarAsync(Webinar webinar)
    {
        await _context.Webinars.AddAsync(webinar);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}