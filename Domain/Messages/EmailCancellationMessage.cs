using Domain.Entities;
namespace Domain.Messages;

public sealed record EmailCancellationMessage(IEnumerable<Person> People);