namespace Domain.Dtos;

public sealed record EmailRegistrationMessage(
    string Name,
    string Email,
    string WebinarTitle,
    string WebinarHost) 
    : IServiceBusMessage;