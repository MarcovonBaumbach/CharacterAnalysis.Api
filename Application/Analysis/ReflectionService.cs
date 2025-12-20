using Microsoft.SemanticKernel;

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
        var prompt = """
You are a thoughtful psychological analyst.

Analyze the following observations and provide:
- a coherent interpretation
- emotional depth
- alternative perspectives
- reflective insight

Observations:
{{$input}}
""";

        var result = await _kernel.InvokePromptAsync(
            prompt,
            new KernelArguments
            {
                ["input"] = observations
            });

        return result.GetValue<string>() ?? string.Empty;
    }
}
