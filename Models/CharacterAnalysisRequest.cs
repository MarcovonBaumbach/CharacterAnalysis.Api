namespace CharacterAnalysis.Api.Models;

public class CharacterAnalysisRequest
{
    public string ShowName { get; set; } = string.Empty;
    public string? Episode { get; set; }
    public string Observations { get; set; } = string.Empty;
}