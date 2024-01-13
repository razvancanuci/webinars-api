namespace Domain.Constants;

public class WebinarConstants
{
    public static readonly DateTime AvailabilityDate = DateTime.UtcNow.AddDays(2);
    public static readonly List<string> AcceptedImageExtensions = [".png", ".jpg"];
}