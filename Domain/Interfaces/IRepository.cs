using Domain.Entities;

namespace Domain.Interfaces;

public interface IRepository
{
    Task GetWebinarsAsync();
    Task GetWebinarByIdAsync(string id);
    Task RegisterPersonToWebinarAsync(Person person, string webinarId);
    Task AddWebinarAsync(Webinar webinar);
}