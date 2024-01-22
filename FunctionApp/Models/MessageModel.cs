using Domain.Messages.Interfaces;

namespace FunctionApp.Models;

public class MessageModel<T> 
where T: IServiceBusMessage
{
    public T Message { get; set; }
}