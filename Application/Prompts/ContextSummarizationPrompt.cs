namespace CharacterAnalysis.Api.Application.Prompts;

public static class ContextSummarizationPrompt
{
    public const string Template = """
    You are summarizing factual information about a TV show, to gather information about deep character insides and psychological dynamics.

    Source material:
    {{$sources}}

    Produce a concise summary that:
    - Focuses on characters, themes, and psychological dynamics
    - Avoids episode-by-episode plot details
    - Avoids spoilers beyond general premise
    - Uses neutral, encyclopedic language
    - Is under 500 words

    Summary:
    """;
}
