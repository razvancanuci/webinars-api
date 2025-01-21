namespace Domain.Entities;

#nullable disable
public class Person : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public string WebinarId { get; set; }

    public Webinar Webinar { get; set; }
}