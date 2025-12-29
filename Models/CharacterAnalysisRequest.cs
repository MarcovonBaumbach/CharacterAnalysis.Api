namespace CharacterAnalysis.Api.Models;

public class CharacterAnalysisRequest
{
    public string Observations { get; set; } = string.Empty;
    public string ShowName { get; set; } = string.Empty;
    public string? Episode { get; set; }
}

