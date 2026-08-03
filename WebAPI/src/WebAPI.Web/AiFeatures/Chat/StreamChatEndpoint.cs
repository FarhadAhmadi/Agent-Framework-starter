using System.Text;
using FastEndpoints;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using WebAPI.Web.AiFeatures.Infrastructure;
using WebAPI.Web.Configurations;

namespace WebAPI.Web.AiFeatures.Chat;

public sealed class StreamChatRequest
{
  public const string Route = "/ai/chat/stream";

  public Guid? ConversationId { get; init; }
  public string Message { get; init; } = string.Empty;
  public string? SystemPrompt { get; init; }
}

// EndpointWithoutResponse: we write the SSE stream manually.
public class StreamChatEndpoint(
  IChatClient chatClient,
  IConversationStore store,
  IOptions<OllamaOptions> options)
  : Endpoint<StreamChatRequest>
{
  private readonly OllamaOptions _options = options.Value;

  public override void Configure()
  {
    Post(StreamChatRequest.Route);
    AllowAnonymous();
    Tags("AI");

    Summary(s =>
    {
      s.Summary = "Stream a chat response (SSE)";
      s.Description = "Streams the model reply token-by-token as text/event-stream. Ends with 'data: [DONE]'.";
    });
  }

  public override async Task HandleAsync(StreamChatRequest req, CancellationToken ct)
  {
    if (string.IsNullOrWhiteSpace(req.Message))
    {
      await Send.ErrorsAsync(400, ct);
      return;
    }

    HttpContext.Response.Headers.ContentType = "text/event-stream";
    HttpContext.Response.Headers.CacheControl = "no-cache";

    var conversation = store.PrepareForPrompt(req.ConversationId, req.Message, req.SystemPrompt, _options);
    var buffer = new StringBuilder();

    await foreach (var update in chatClient.GetStreamingResponseAsync(conversation.Messages, cancellationToken: ct))
    {
      var text = update.Text;
      if (string.IsNullOrEmpty(text))
      {
        continue;
      }
      buffer.Append(text);
      var payload = text.Replace("\n", "\\n");
      await HttpContext.Response.WriteAsync($"data: {payload}\n\n", ct);
      await HttpContext.Response.Body.FlushAsync(ct);
    }

    conversation.Messages.Add(new ChatMessage(ChatRole.Assistant, buffer.ToString()));
    await HttpContext.Response.WriteAsync("data: [DONE]\n\n", ct);
  }
}
