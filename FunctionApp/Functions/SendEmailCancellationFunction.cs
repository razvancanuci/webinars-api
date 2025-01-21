using System.Text;
using Azure.Messaging.ServiceBus;
using Domain.Messages;
using FunctionApp.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp.Functions;

public class SendEmailCancellationFunction
{
    private readonly ILogger<SendEmailCancellationFunction> _logger;

    public SendEmailCancellationFunction(ILogger<SendEmailCancellationFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(SendEmailCancellationFunction))]
    public void Run([ServiceBusTrigger("send-emails-topic",
        "send-email-cancellation-subscription",
        Connection = "ServiceBus:ConnectionString")] ServiceBusReceivedMessage message)
    {
        var emailMessage = JsonConvert.DeserializeObject<MessageModel<EmailCancellationMessage>>(Encoding.UTF8.GetString(message.Body))?.Message;
        
        _logger.LogInformation("Name {name}",emailMessage!.WebinarId);
    }
}