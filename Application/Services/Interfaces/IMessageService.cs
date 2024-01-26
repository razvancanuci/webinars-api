namespace Application.Services.Interfaces;

public interface IMessageService
{
    Task Send<T>(T message) where T : class;
}