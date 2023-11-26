using Domain.Entities;
using Domain.Interfaces;

namespace DataAccess.Repositories;

public class WebinarRepository : IRepository
{
    public Task GetWebinarsAsync()
    {
        return Task.CompletedTask;
    }

    public Task GetWebinarByIdAsync(int id)
    {
        return Task.CompletedTask;
    }

    public Task RegisterPersonToWebinarAsync(Person person, int webinarId)
    {
        return Task.CompletedTask;
    }
}