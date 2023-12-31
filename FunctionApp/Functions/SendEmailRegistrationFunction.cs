using System;
using System.Text;
using Azure.Messaging.ServiceBus;
using Domain.Dtos;
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
        var emailDto = JsonConvert.DeserializeObject<SendEmailRegistrationModel>(Encoding.UTF8.GetString(message.Body))?.Message;
        
        _logger.LogInformation("Name {name}",emailDto?.Name);
        _logger.LogInformation("Email {email}",emailDto?.Email);
        _logger.LogInformation("Title {title}",emailDto?.WebinarTitle);
        _logger.LogInformation("Host {host}",emailDto?.WebinarHost);
        
    }
}