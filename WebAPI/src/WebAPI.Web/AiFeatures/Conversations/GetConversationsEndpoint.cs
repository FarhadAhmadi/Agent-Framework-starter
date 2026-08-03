using FastEndpoints;
using WebAPI.Web.AiFeatures.Infrastructure;

namespace WebAPI.Web.AiFeatures.Conversations;

public class GetConversationsEndpoint(IConversationStore store)
  : EndpointWithoutRequest<IReadOnlyList<ConversationSummaryResponse>>
{
  public override void Configure()
  {
    Get("/ai/conversations");
    AllowAnonymous();
    Tags("AI");
    Summary(s => s.Summary = "List all conversations");
  }

  public override Task HandleAsync(CancellationToken ct)
  {
    var summaries = store.List()
      .Select(c => new ConversationSummaryResponse(c.Id, c.CreatedAt, c.Messages.Count))
      .ToList();
    return Send.OkAsync(summaries, ct);
  }
}
