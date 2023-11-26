using Domain.Entities;

namespace Domain.Interfaces;

public interface IRepository
{
    Task<IEnumerable<Webinar>> GetWebinarsAsync(int page, int itemsPerPage);
    Task<Webinar?> GetWebinarByIdAsync(string id);
    Task RegisterPersonToWebinarAsync(Webinar webinar);
    Task AddWebinarAsync(Webinar webinar);
}