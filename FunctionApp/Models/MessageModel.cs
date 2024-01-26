namespace FunctionApp.Models;

public class MessageModel<T> 
where T: class
{
    public T Message { get; set; }
}