using WebAPI.Web.Configurations;

namespace WebAPI.Web.AiFeatures.Infrastructure;

public interface IConversationStore
{
  Conversation GetOrCreate(Guid? id);
  Conversation? Get(Guid id);
  IReadOnlyCollection<Conversation> List();
  bool Delete(Guid id);
  // Ensures a system prompt exists, then appends the user's message.
  Conversation PrepareForPrompt(Guid? id, string userMessage, string? systemPrompt, OllamaOptions options);
}
