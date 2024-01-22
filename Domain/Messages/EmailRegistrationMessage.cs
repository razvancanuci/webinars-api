using Domain.Messages.Interfaces;

namespace Domain.Messages;

public sealed record EmailRegistrationMessage(
    string Name,
    string Email,
    string WebinarTitle,
    string WebinarHost) 
    : IServiceBusMessage;