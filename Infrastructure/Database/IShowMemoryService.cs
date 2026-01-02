using CharacterAnalysis.Api.Models;

namespace CharacterAnalysis.Api.Infrastructure.Database;

public interface IShowMemoryService
{
    Task<TvShowEntity> GetOrCreateShowAsync(string showName);
    Task<IReadOnlyList<TvShowEntity>> GetAllShowsAsync();
    Task<IReadOnlyList<EpisodeMemoryEntity>> GetEpisodesAsync(Guid showId);
    Task<EpisodeMemoryEntity> SaveEpisodeAsync(EpisodeMemoryEntity episode);
}