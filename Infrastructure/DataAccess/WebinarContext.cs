using System.Diagnostics.CodeAnalysis;
using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DataAccess;

[ExcludeFromCodeCoverage]
public class WebinarContext : DbContext
{
    public DbSet<Webinar> Webinars { get; set; }

    public DbSet<Person> PeopleRegistered { get; set; }

    public WebinarContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Webinar>().ToContainer("Webinars");

        modelBuilder.Entity<Person>().ToContainer("PeopleRegistered");

        modelBuilder.Entity<Webinar>().Property(x => x.Id)
            .HasValueGenerator<StringValueGenerator>();
        
        modelBuilder.Entity<Webinar>()
            .HasPartitionKey(x => x.Host);
        
        modelBuilder.Entity<Webinar>()
            .HasQueryFilter(w => w.ScheduledAt > WebinarConstants.AvailabilityDate);
        
        modelBuilder.Entity<Webinar>()
            .HasQueryFilter(w => !w.IsDeleted);

        modelBuilder.Entity<Person>().HasKey(x => x.Id);
        modelBuilder.Entity<Person>().Property(x => x.Id)
            .HasValueGenerator<StringValueGenerator>();
        
        modelBuilder.Entity<Person>()
            .HasPartitionKey(x => x.WebinarId);

        modelBuilder.Entity<Person>().HasQueryFilter(x => !x.Webinar.IsDeleted);

        modelBuilder.Entity<Webinar>().HasKey(x => x.Id);
        modelBuilder.Entity<Webinar>().Property(x => x.Host).IsRequired();
        modelBuilder.Entity<Webinar>().Property(x => x.Title).HasMaxLength(30);
        
        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(CosmosEventId.SyncNotSupported));
    }

}