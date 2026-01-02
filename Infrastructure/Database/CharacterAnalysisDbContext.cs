using CharacterAnalysis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CharacterAnalysis.Api.Infrastructure.Database;

public class CharacterAnalysisDbContext : DbContext
{
    public CharacterAnalysisDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<TvShowEntity> TvShows => Set<TvShowEntity>();
    public DbSet<EpisodeMemoryEntity> EpisodeMemories => Set<EpisodeMemoryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TvShowEntity>()
            .HasIndex(s => s.Name)
            .IsUnique();

        modelBuilder.Entity<EpisodeMemoryEntity>()
            .HasOne(e => e.TvShow)
            .WithMany(s => s.Episodes)
            .HasForeignKey(e => e.TvShowId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
