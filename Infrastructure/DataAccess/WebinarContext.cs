using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DataAccess;

public class WebinarContext : DbContext
{
    public DbSet<Webinar> Webinars { get; set; }

    public WebinarContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Webinar>().ToContainer("Webinars").HasPartitionKey(x => x.Host);

        modelBuilder.Entity<Webinar>().Property(x => x.Id).HasValueGenerator<StringValueGenerator>();
        base.OnModelCreating(modelBuilder);
    }
}