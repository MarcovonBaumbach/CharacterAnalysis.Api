namespace CharacterAnalysis.Api.Context;

public interface IShowContextCache
{
    Task<ShowContext?> GetAsync(string showName, string? episode);
    Task SetAsync(ShowContext context);
}
