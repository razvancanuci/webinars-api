using System.Text;
using Azure.Messaging.ServiceBus;
using Domain.Messages;
using FunctionApp.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp.Functions;

public class SendEmailRegistrationFunction
{
    private readonly ILogger<SendEmailRegistrationFunction> _logger;

    public SendEmailRegistrationFunction(ILogger<SendEmailRegistrationFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(SendEmailRegistrationFunction))]
    public void Run([ServiceBusTrigger("send-emails-topic",
        "send-email-registration-subscription",
        Connection = "ServiceBus:ConnectionString")] ServiceBusReceivedMessage message)
    {
        var emailMessage = JsonConvert.DeserializeObject<MessageModel<EmailRegistrationMessage>>(Encoding.UTF8.GetString(message.Body))?.Message;
        
        _logger.LogInformation("Name {name}",emailMessage!.Name);
        _logger.LogInformation("Email {email}",emailMessage.Email);
        _logger.LogInformation("Title {title}",emailMessage.WebinarTitle);
        _logger.LogInformation("Host {host}",emailMessage.WebinarHost);
        
    }
}