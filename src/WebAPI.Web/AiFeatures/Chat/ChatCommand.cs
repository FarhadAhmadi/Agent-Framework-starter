using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using WebAPI.Web.AiFeatures.Infrastructure;
using WebAPI.Web.Configurations;

namespace WebAPI.Web.AiFeatures.Chat;

public record ChatCommand(Guid? ConversationId, string Message, string? SystemPrompt)
  : ICommand<Result<ChatResultDto>>;

public class ChatHandler(
  IChatClient chatClient,
  IConversationStore store,
  IOptions<OllamaOptions> options)
  : ICommandHandler<ChatCommand, Result<ChatResultDto>>
{
  private readonly OllamaOptions _options = options.Value;

  public async ValueTask<Result<ChatResultDto>> Handle(ChatCommand request, CancellationToken cancellationToken)
  {
    var conversation = store.PrepareForPrompt(
      request.ConversationId, request.Message, request.SystemPrompt, _options);

    var response = await chatClient.GetResponseAsync(conversation.Messages, cancellationToken: cancellationToken);
    var reply = response.Text ?? string.Empty;

    conversation.Messages.Add(new ChatMessage(ChatRole.Assistant, reply));

    return new ChatResultDto(conversation.Id, reply, _options.DefaultModel);
  }
}
