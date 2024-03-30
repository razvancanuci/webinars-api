namespace Domain.Interfaces;

public interface IUnitOfWork
{
    public IWebinarRepository WebinarRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
}