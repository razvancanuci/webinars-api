namespace Domain.Interfaces;

public interface IMessageService
{
    Task Send<T>(T message, CancellationToken cancellationToken = default) where T : class;
}