using Microsoft.EntityFrameworkCore;
using WeatherForecast.Database.Models;

namespace WeatherForecast.Database;

public class ArchivesContext : DbContext
{
    public DbSet<Data> WeatherData { get; set; }
    
    public ArchivesContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Data>()
            .HasIndex(x => new { x.Date })
            .IsUnique();
    }
}