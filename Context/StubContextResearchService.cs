namespace CharacterAnalysis.Api.Context;

public sealed class StubContextResearchService : IContextResearchService
{
    public Task<ShowContext> GetShowContextAsync(
        string showName,
        string? episode = null)
    {
        return Task.FromResult(new ShowContext
        {
            ShowName = showName,
            Episode = episode,
            Summary = """
                This show focuses on interpersonal dynamics,
                emotional conflict, and identity formation.
                Characters often struggle with communication,
                unresolved past experiences, and value clashes.
                """
        });
    }
}
