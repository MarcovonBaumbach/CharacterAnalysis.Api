using System.Text;
using System.Text.Json;

namespace CharacterAnalysis.Api.Context;

public sealed class TavilyContextResearchService : IContextResearchService
{
    private readonly HttpClient _http;
    private readonly TavilyOptions _options;

    public TavilyContextResearchService(
        HttpClient http,
        TavilyOptions options)
    {
        _http = http;
        _options = options;
    }

    public async Task<ShowContext> GetShowContextAsync(
        string showName,
        string? episode = null)
    {
        var query = BuildQuery(showName, episode);

        var requestBody = new
        {
            api_key = _options.ApiKey,
            query = query,
            search_depth = "advanced",
            include_answer = true,
            max_results = 5
        };

        var response = await _http.PostAsync(
            "https://api.tavily.com/search",
            new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"));

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var summary = ExtractSummary(json);

        return new ShowContext
        {
            ShowName = showName,
            Episode = episode,
            Summary = summary
        };
    }

    private static string BuildQuery(string showName, string? episode)
    {
        if (!string.IsNullOrWhiteSpace(episode))
        {
            return $"{showName} {episode} character analysis themes psychology";
        }

        return $"{showName} tv show character psychology themes analysis";
    }

    private static string ExtractSummary(string json)
    {
        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.TryGetProperty("answer", out var answer))
        {
            return answer.GetString() ?? string.Empty;
        }

        return string.Empty;
    }
}
