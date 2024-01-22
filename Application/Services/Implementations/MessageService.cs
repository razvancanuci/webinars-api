using Application.Services.Interfaces;
using Domain.Messages.Interfaces;
using MassTransit;

namespace Application.Services.Implementations;

public class MessageService : IMessageService
{
    private readonly IPublishEndpoint _publisher;
    
    public MessageService(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }
    
    public async Task Send<T>(T message) where T : IServiceBusMessage
    {
        await _publisher.Publish(message);
    }
}