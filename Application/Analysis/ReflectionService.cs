using Microsoft.SemanticKernel;
using CharacterAnalysis.Api.Application.Prompts;

namespace CharacterAnalysis.Api.Application.Analysis;

public class ReflectionService
{
    private readonly Kernel _kernel;

    public ReflectionService(Kernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<string> ReflectAsync(string observations)
    {
        var newPrompt = ReflectionPrompt.Template;

        var result = await _kernel.InvokePromptAsync(
            newPrompt,
            new KernelArguments
            {
                ["input"] = observations
            });

        return result.GetValue<string>() ?? string.Empty;
    }
}
