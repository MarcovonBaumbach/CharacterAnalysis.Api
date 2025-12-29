namespace CharacterAnalysis.Api.Context;

public interface IContextResearchService
{
    Task<ShowContext> GetShowContextAsync(
        string showName,
        string? episode = null);
}
