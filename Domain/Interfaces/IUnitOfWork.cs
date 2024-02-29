using Domain.Entities;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    public IWebinarRepository WebinarRepository { get; }
    Task SaveAsync();
}