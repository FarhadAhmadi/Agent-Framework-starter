// IAIService.cs + OllamaAgentService.cs
using Microsoft.Extensions.AI;

namespace Agent_Framework;

public interface IAIService
{
    /// <summary>Sends a prompt and returns the model's text reply.</summary>
    Task<string> GetResponseAsync(string prompt, CancellationToken ct = default);
}

public class OllamaAgentService(IChatClient chatClient) : IAIService
{
    /// <inheritdoc/>
    public async Task<string> GetResponseAsync(string prompt, CancellationToken ct = default)
    {
        // GetResponseAsync returns a ChatResponse; .Text is the convenience
        // property for the last assistant message's text content.
        var response = await chatClient.GetResponseAsync(prompt, cancellationToken: ct);
        return response.Text ?? string.Empty;
    }
}
