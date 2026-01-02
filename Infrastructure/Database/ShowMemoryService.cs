using CharacterAnalysis.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CharacterAnalysis.Api.Infrastructure.Database;

public sealed class ShowMemoryService : IShowMemoryService
{
    private readonly CharacterAnalysisDbContext _db;

    public ShowMemoryService(CharacterAnalysisDbContext db)
    {
        _db = db;
    }

    public async Task<TvShowEntity> GetOrCreateShowAsync(string showName)
    {
        var show = await _db.TvShows
            .FirstOrDefaultAsync(s => s.Name == showName);

        if (show != null)
            return show;

        show = new TvShowEntity
        {
            Id = Guid.NewGuid(),
            Name = showName,
            CreatedAt = DateTime.UtcNow
        };

        _db.TvShows.Add(show);
        await _db.SaveChangesAsync();

        return show;
    }

    public async Task<EpisodeMemoryEntity> SaveEpisodeAsync(EpisodeMemoryEntity episode)
    {
        _db.EpisodeMemories.Add(episode);
        await _db.SaveChangesAsync();
        
        return episode;
    }

    public async Task<IReadOnlyList<TvShowEntity>> GetAllShowsAsync()
        => await _db.TvShows
            .OrderBy(s => s.Name)
            .ToListAsync();

    public async Task<IReadOnlyList<EpisodeMemoryEntity>> GetEpisodesAsync(Guid showId)
        => await _db.EpisodeMemories
            .Where(e => e.TvShowId == showId)
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
}
