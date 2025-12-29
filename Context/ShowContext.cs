namespace CharacterAnalysis.Api.Context;

public sealed class ShowContext
{
    public string ShowName { get; init; } = string.Empty;
    public string? Episode { get; init; }
    public string Summary { get; init; } = string.Empty;
}
