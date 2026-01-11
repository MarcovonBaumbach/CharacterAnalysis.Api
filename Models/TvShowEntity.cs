namespace CharacterAnalysis.Api.Models;

public class TvShowEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<EpisodeMemoryEntity> Episodes { get; set; } = new();
}
