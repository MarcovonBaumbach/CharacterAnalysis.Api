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
            e.CreatedAt
        }));
    }

    [HttpPost("reflection")]
    public async Task<IActionResult> Post([FromBody] CharacterAnalysisRequest request)
    {
        var show = await _showMemoryService
            .GetOrCreateShowAsync(request.ShowName);

        var reflection = await _reflectionService.ReflectAsync(
            request.ShowName,
            request.Observations,
            request.Episode);

        var contextSnapshot = reflection;

        await _showMemoryService.SaveEpisodeAsync(new EpisodeMemoryEntity
        {
            Id = Guid.NewGuid(),
            TvShowId = show.Id,
            Episode = request.Episode ?? "Unknown",
            Observations = request.Observations,
            ContextSnapshot = contextSnapshot,
            Reflection = reflection,
            CreatedAt = DateTime.UtcNow
        });

        //var result = new CharacterAnalysisResponse
        //{
        //    Reflection = reflection
        //};

        return Ok(new
        {
            showId = show.Id,
            reflection
        });
    }
}
