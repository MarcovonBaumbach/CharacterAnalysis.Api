using Microsoft.AspNetCore.Mvc;
using CharacterAnalysis.Api.Application.Analysis;
using CharacterAnalysis.Api.Models;
using CharacterAnalysis.Api.Infrastructure.Database;

[ApiController]
[Route("api")]
public class ReflectionController : ControllerBase
{
    private readonly ReflectionService _reflectionService;
    private readonly IShowMemoryService _showMemoryService;

    public ReflectionController(
        ReflectionService reflectionService,
        IShowMemoryService showMemoryService)
    {
        _reflectionService = reflectionService;
        _showMemoryService = showMemoryService;
    }

    [HttpGet("shows")]
    public async Task<IActionResult> GetShows()
    {
        var shows = await _showMemoryService.GetAllShowsAsync();

        return Ok(shows.Select(s => new
        {
            s.Id,
            s.Name
        }));
    }

    [HttpGet("shows/{showId}/episodes")]
    public async Task<IActionResult> GetEpisodes(Guid showId)
    {
        var episodes = await _showMemoryService
            .GetEpisodesAsync(showId);

        return Ok(episodes.Select(e => new
        {
            e.Id,
            e.Episode,
            e.Observations,
            e.ContextSnapshot,
            e.Reflection,
            e.CreatedAt
        }));
    }

    [HttpPost("reflection")]
    public async Task<IActionResult> Post([FromBody] CharacterAnalysisRequest request)
    {
        var show = await _showMemoryService
            .GetOrCreateShowAsync(request.ShowName);

        var pastEpisodes = await _showMemoryService.GetEpisodesAsync(show.Id);

        var recentEpisodes = pastEpisodes
            .OrderByDescending(e => e.CreatedAt)
            .Take(10)
            .OrderBy(e => e.CreatedAt)
            .ToList();

        var previousEpisodesContext = string.Join(
            "\n\n---\n\n",
            recentEpisodes.Select(e =>
                $"Episode: {e.Episode}\n" +
                $"Observations: {e.Observations}\n" +
                $"Reflection: {e.Reflection}")
        );

        var reflection = await _reflectionService.ReflectAsync(
            request.ShowName,
            request.Observations,
            request.Episode,
            previousEpisodesContext);

        await _showMemoryService.SaveEpisodeAsync(new EpisodeMemoryEntity
        {
            Id = Guid.NewGuid(),
            TvShowId = show.Id,
            Episode = request.Episode ?? "Unknown",
            Observations = request.Observations,
            ContextSnapshot = reflection.ContextSnapshot,
            Reflection = reflection.Reflection,
            CreatedAt = DateTime.UtcNow
        });

        return Ok(new
        {
            showId = show.Id,
            reflection
        });
    }
}
