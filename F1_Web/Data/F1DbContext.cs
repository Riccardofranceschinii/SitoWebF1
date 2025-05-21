using System;
using F1_Web.Model;
using Microsoft.EntityFrameworkCore;

namespace F1_Web.Data;

public class F1DbContext : DbContext
{
    public F1DbContext(DbContextOptions<F1DbContext> options) : base(options) { }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Constructor> Constructors { get; set; }
    public DbSet<Circuit> Circuits { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Circuit>()
            .OwnsOne(c => c.Location);
    }
}
