using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel;
using CharacterAnalysis.Api.Application.Prompts;

namespace CharacterAnalysis.Api.Context;

public sealed class TavilyContextResearchService : IContextResearchService
{
    private readonly HttpClient _http;
    private readonly TavilyOptions _options;
    private readonly Kernel _kernel;
    private readonly IShowContextCache _cache;

    public TavilyContextResearchService(
        HttpClient http,
        TavilyOptions options,
        Kernel kernel,
        IShowContextCache cache)
    {
        _http = http;
        _options = options;
        _kernel = kernel;
        _cache = cache;
    }

    public async Task<ShowContext> GetShowContextAsync(
        string showName,
        string? episode = null)
    {
        var cached = await _cache.GetAsync(showName, episode);
        if (cached != null)
            return cached;

        var query = BuildQuery(showName, episode);
        var snippets = await FetchSnippetsAsync(query);
        var summary = await SummarizeAsync(snippets);

        var context = new ShowContext
        {
            ShowName = showName,
            Episode = episode,
            Summary = summary
        };

        await _cache.SetAsync(context);
        return context;
    }

    private async Task<IReadOnlyList<string>> FetchSnippetsAsync(string query)
    {
        var requestBody = new
        {
            api_key = _options.ApiKey,
            query = query,
            search_depth = "advanced",
            include_answer = false,
            max_results = 6
        };

        var response = await _http.PostAsync(
            "https://api.tavily.com/search",
            new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"));

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return ExtractSnippets(json);
    }

    private static IReadOnlyList<string> ExtractSnippets(string json)
    {
        using var doc = JsonDocument.Parse(json);
        var results = doc.RootElement.GetProperty("results");

        var snippets = new List<string>();

        foreach (var result in results.EnumerateArray())
        {
            if (result.TryGetProperty("content", out var content))
            {
                var text = content.GetString();
                if (!string.IsNullOrWhiteSpace(text))
                    snippets.Add(text);
            }
        }

        return snippets;
    }

    private async Task<string> SummarizeAsync(IReadOnlyList<string> sources)
    {
        var joined = string.Join("\n\n---\n\n", sources);

        var result = await _kernel.InvokePromptAsync(
            ContextSummarizationPrompt.Template,
            new KernelArguments
            {
                ["sources"] = joined
            });

        return result.GetValue<string>() ?? string.Empty;
    }

    private static string BuildQuery(string showName, string? episode)
    {
        return episode is null
            ? $"{showName} tv show character psychology themes analysis"
            : $"{showName} {episode} character analysis themes psychology";
    }
}
