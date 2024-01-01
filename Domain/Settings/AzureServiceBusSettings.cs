namespace Domain.Settings;

public class AzureServiceBusSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string SendEmailTopicName { get; set; } = string.Empty;
}