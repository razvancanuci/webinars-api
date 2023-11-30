﻿using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DataAccess;

public class WebinarContext : DbContext
{
    public DbSet<Webinar> Webinars { get; set; }

    public WebinarContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Webinar>().ToContainer("Webinars");

        modelBuilder.Entity<Webinar>().Property(x => x.Id).HasValueGenerator<StringValueGenerator>();
        modelBuilder.Entity<Webinar>().HasPartitionKey(x => x.Host);

        modelBuilder.Entity<Webinar>().HasKey(x => x.Id);
        modelBuilder.Entity<Webinar>().Property(x => x.Host).IsRequired();
        modelBuilder.Entity<Webinar>().HasIndex(x => x.Title).IsUnique();
        modelBuilder.Entity<Webinar>().Property(x => x.Title).HasMaxLength(30);
        
        base.OnModelCreating(modelBuilder);
    }
}