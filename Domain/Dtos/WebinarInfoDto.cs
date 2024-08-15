namespace Domain.Dtos;

public sealed record WebinarInfoDto
{
    public string Title { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
}