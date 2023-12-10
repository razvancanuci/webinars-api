using DataAccess;
using Domain.Entities;

namespace IntegrationTests;

public static class DatabaseSeed
{
    public static void InitializeDbForTests(WebinarContext context)
    {
        context.Webinars.AddRange(GetSeedWebinars());
        context.SaveChanges();
    }

    private static IEnumerable<Webinar> GetSeedWebinars()
    {
        return new List<Webinar>
        {
            new Webinar
            {
                Id = Guid.Parse("e6e56821-0284-4f10-aafa-167e1c8f5868").ToString(),
                Title = "Good webinar",
                Description = "Very good webinar come here",
                Host = "Lup",
                ScheduleDate = DateTime.UtcNow.AddDays(15)
            },
            new Webinar
            {
                Id = Guid.Parse("b21d1afb-7dc5-478f-b260-76e5f7af79ef").ToString(),
                Title = "Good webinar2",
                Description = "Very good webinar come here2",
                Host = "Lup",
                ScheduleDate = DateTime.UtcNow.AddDays(20),
            }
        };
    }
}