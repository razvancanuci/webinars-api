using Domain.Entities;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    public IRepository<Webinar> WebinarRepository { get; }
    Task SaveAsync();
}