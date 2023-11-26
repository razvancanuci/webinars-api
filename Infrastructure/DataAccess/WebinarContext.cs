using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class WebinarContext : DbContext
{
    public DbSet<Webinar> Webinars { get; set; }

    public WebinarContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}