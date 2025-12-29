using Microsoft.SemanticKernel;
using CharacterAnalysis.Api.Application.Prompts;
using CharacterAnalysis.Api.Context;


namespace CharacterAnalysis.Api.Application.Analysis;

public class ReflectionService
{
    private readonly Kernel _kernel;
    private readonly IContextResearchService _contextResearch;

    public ReflectionService(
        Kernel kernel,
        IContextResearchService contextResearch)
    {
        _kernel = kernel;
        _contextResearch = contextResearch;
    }

    public async Task<string> ReflectAsync(
        string showName,
        string observations,
        string? episode = null)
    {
        var context = await _contextResearch
            .GetShowContextAsync(showName, episode);

        var result = await _kernel.InvokePromptAsync(
            ReflectionPrompt.Template,
            new KernelArguments
            {
                ["context"] = context.Summary,
                ["input"] = observations
            });

        return result.GetValue<string>() ?? string.Empty;
    }
}
