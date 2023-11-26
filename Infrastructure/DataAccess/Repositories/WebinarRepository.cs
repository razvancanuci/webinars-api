using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.Repositories;

public class WebinarRepository : IRepository
{
    public WebinarRepository()
    {
    }
    public Task GetWebinarsAsync()
    {
        return Task.CompletedTask;
    }

    public Task GetWebinarByIdAsync(string id)
    {
        return Task.CompletedTask;
    }

    public Task RegisterPersonToWebinarAsync(Person person, string webinarId)
    {
        return Task.CompletedTask;
    }

    public Task AddWebinarAsync(Webinar webinar)
    {
        return Task.CompletedTask;
    }
}