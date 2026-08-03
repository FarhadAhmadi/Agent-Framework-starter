using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using WebAPI.Web.AiFeatures.Infrastructure;

namespace WebAPI.Web.AiFeatures.Conversations;

public sealed class DeleteConversationRequest
{
  public const string Route = "/ai/conversations/{ConversationId}";
  public Guid ConversationId { get; init; }
}

public class DeleteConversationEndpoint(IConversationStore store)
  : Endpoint<DeleteConversationRequest, Results<NoContent, NotFound>>
{
  public override void Configure()
  {
    Delete(DeleteConversationRequest.Route);
    AllowAnonymous();
    Tags("AI");
    Summary(s => s.Summary = "Delete a conversation");
  }

  public override async Task<Results<NoContent, NotFound>>
    ExecuteAsync(DeleteConversationRequest request, CancellationToken ct)
  {
    var deleted = store.Delete(request.ConversationId);
    return await Task.FromResult<Results<NoContent, NotFound>>(
      deleted ? TypedResults.NoContent() : TypedResults.NotFound());
  }
}
