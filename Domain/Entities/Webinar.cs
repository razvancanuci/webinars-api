using Domain.Constants;

namespace Domain.Entities;

#nullable disable

public class Webinar : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Host { get; set; }
    public DateTime ScheduledAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public List<Person> PeopleRegistered { get; set; } = new();

    public bool IsDeleted { get; set; }

    public bool IsAvailable() => ScheduledAt > WebinarConstants.AvailabilityDate;
}