using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebAPI.Web.AiFeatures.Infrastructure;

namespace WebAPI.Web.AiFeatures.Conversations;

public sealed class GetConversationRequest
{
  public const string Route = "/ai/conversations/{ConversationId}";
  public Guid ConversationId { get; init; }
}

public class GetConversationByIdEndpoint(IConversationStore store)
  : Endpoint<GetConversationRequest, Results<Ok<IReadOnlyList<MessageResponse>>, NotFound>>
{
  public override void Configure()
  {
    Get(GetConversationRequest.Route);
    AllowAnonymous();
    Tags("AI");
    Summary(s => s.Summary = "Get conversation messages by ID");
  }

  public override async Task<Results<Ok<IReadOnlyList<MessageResponse>>, NotFound>>
    ExecuteAsync(GetConversationRequest request, CancellationToken ct)
  {
    var conversation = store.Get(request.ConversationId);
    if (conversation is null)
    {
      return TypedResults.NotFound();
    }

    IReadOnlyList<MessageResponse> messages = conversation.Messages
      .Select(m => new MessageResponse(m.Role.Value, m.Text))
      .ToList();

    return await Task.FromResult(TypedResults.Ok(messages));
  }
}
