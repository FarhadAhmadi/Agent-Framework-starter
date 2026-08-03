using System.Collections.Concurrent;
using Microsoft.Extensions.AI;
using WebAPI.Web.Configurations;

namespace WebAPI.Web.AiFeatures.Infrastructure;

public sealed class InMemoryConversationStore : IConversationStore
{
  private readonly ConcurrentDictionary<Guid, Conversation> _store = new();

  public Conversation GetOrCreate(Guid? id)
  {
    if (id is { } existing && _store.TryGetValue(existing, out var found))
    {
      return found;
    }
    var conversation = new Conversation();
    _store[conversation.Id] = conversation;
    return conversation;
  }

  public Conversation? Get(Guid id) => _store.GetValueOrDefault(id);

  public IReadOnlyCollection<Conversation> List() => _store.Values.ToArray();

  public bool Delete(Guid id) => _store.TryRemove(id, out _);

  public Conversation PrepareForPrompt(Guid? id, string userMessage, string? systemPrompt, OllamaOptions options)
  {
    var conversation = GetOrCreate(id);
    if (conversation.Messages.Count == 0)
    {
      var prompt = string.IsNullOrWhiteSpace(systemPrompt) ? options.DefaultSystemPrompt : systemPrompt;
      conversation.Messages.Add(new ChatMessage(ChatRole.System, prompt));
    }
    conversation.Messages.Add(new ChatMessage(ChatRole.User, userMessage));
    return conversation;
  }
}
