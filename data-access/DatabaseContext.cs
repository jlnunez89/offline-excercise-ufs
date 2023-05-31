using data.Entities;
using Microsoft.EntityFrameworkCore;

namespace data;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }

    public DbSet<AverageAge> AverageAges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.ToTable("players");
            entity.Property(e => e.Id);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(e => e.NameBrief)
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(e => e.Position)
                .IsRequired()
                .HasMaxLength(16);
            entity.Property(e => e.Age);
            entity.Property(e => e.Sport)
                .IsRequired()
                .HasMaxLength(32);
        });

        modelBuilder.Entity<AverageAge>(entity =>
        {
            entity.ToTable("average_ages");
            entity.HasKey(a => new { a.Sport, a.Position } );
            entity.Property(e => e.Sport)
                .IsRequired()
                .HasMaxLength(32);
            entity.Property(e => e.Position)
                .IsRequired()
                .HasMaxLength(16);
            entity.Property(e => e.Age)
                .IsRequired();
        });
    }
}