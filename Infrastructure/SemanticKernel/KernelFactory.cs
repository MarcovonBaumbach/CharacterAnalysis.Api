using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Configuration;

namespace CharacterAnalysis.Infrastructure.SemanticKernel;

public static class KernelFactory
{
    public static Kernel Create(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("OpenAI API key missing");

        var builder = Kernel.CreateBuilder();

        builder.AddOpenAIChatCompletion(
            modelId: "gpt-4o-mini",
            apiKey: apiKey
        );

        return builder.Build();
    }
}
