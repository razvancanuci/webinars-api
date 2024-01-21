using Domain.Entities;

namespace Domain.Dtos;

public sealed record EmailCancellationMessage(IEnumerable<Person> People) : IServiceBusMessage;