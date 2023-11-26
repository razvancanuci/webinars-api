using Domain.Entities;

namespace Domain.Interfaces;

public interface IRepository
{
    Task GetWebinarsAsync();
    Task GetWebinarByIdAsync(int id);
    Task RegisterPersonToWebinarAsync(Person person, int webinarId);
}