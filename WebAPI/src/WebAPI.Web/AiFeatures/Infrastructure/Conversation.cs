using Microsoft.Extensions.AI;

namespace WebAPI.Web.AiFeatures.Infrastructure;

public sealed class Conversation
{
  public Guid Id { get; init; } = Guid.NewGuid();
  public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
  public List<ChatMessage> Messages { get; } = [];
}
