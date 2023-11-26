namespace Domain.Entities;

#nullable disable

public class Webinar : Entity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Host { get; set; }
    public DateTime ScheduleDate { get; set; }
    public IEnumerable<Person> PeopleRegistered { get; set; } = new List<Person>();
}