namespace CharacterAnalysis.Api.Models;

public class EpisodeMemoryEntity
{
    public Guid Id { get; set; }

    public Guid TvShowId { get; set; }
    public TvShowEntity TvShow { get; set; } = null!;

    public string Episode { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;
    public string ContextSnapshot { get; set; } = string.Empty;
    public string Reflection { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
