namespace CharacterAnalysis.Api.Context;

public sealed class InMemoryShowContextCache : IShowContextCache
{
    private readonly Dictionary<string, ShowContext> _cache = new();

    public Task<ShowContext?> GetAsync(string showName, string? episode)
    {
        _cache.TryGetValue(Key(showName, episode), out var context);
        return Task.FromResult(context);
    }

    public Task SetAsync(ShowContext context)
    {
        _cache[Key(context.ShowName, context.Episode)] = context;
        return Task.CompletedTask;
    }

    private static string Key(string show, string? episode)
        => $"{show}:{episode}";
}
