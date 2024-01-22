using Domain.Entities;
using Domain.Messages.Interfaces;

namespace Domain.Messages;

public sealed record EmailCancellationMessage(IEnumerable<Person> People) : IServiceBusMessage;