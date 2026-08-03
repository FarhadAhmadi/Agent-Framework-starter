using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebAPI.Web.AiFeatures.Chat;

public sealed class ChatEndpointRequest
{
  public const string Route = "/ai/chat";

  public Guid? ConversationId { get; init; }
  public string Message { get; init; } = string.Empty;
  public string? SystemPrompt { get; init; }
}

public class ChatEndpoint(IMediator mediator)
  : Endpoint<ChatEndpointRequest,
             Results<Ok<ChatResponse>, ProblemHttpResult>,
             ChatMapper>
{
  public override void Configure()
  {
    Post(ChatEndpointRequest.Route);
    AllowAnonymous();
    Tags("AI");

    Summary(s =>
    {
      s.Summary = "Send a chat message";
      s.Description = "Sends a message to the Ollama model. Omit ConversationId to start a new conversation.";
      s.ExampleRequest = new ChatEndpointRequest { Message = "Explain CQRS in one sentence." };
      s.Responses[200] = "Model reply returned successfully";
      s.Responses[400] = "Invalid request or Ollama unreachable";
    });

    Description(b => b
      .Accepts<ChatEndpointRequest>("application/json")
      .Produces<ChatResponse>(200, "application/json")
      .ProducesProblem(400));
  }

  public override async Task<Results<Ok<ChatResponse>, ProblemHttpResult>>
    ExecuteAsync(ChatEndpointRequest request, CancellationToken ct)
  {
    var command = new ChatCommand(request.ConversationId, request.Message, request.SystemPrompt);
    var result = await mediator.Send(command, ct);

    if (!result.IsSuccess)
    {
      return TypedResults.Problem(result.Errors.FirstOrDefault() ?? "An error occurred");
    }

    return TypedResults.Ok(Map.FromEntity(result.Value));
  }
}

public sealed class ChatValidator : Validator<ChatEndpointRequest>
{
  public ChatValidator()
  {
    RuleFor(x => x.Message)
      .NotEmpty()
      .WithMessage("Message is required.");
  }
}

public sealed class ChatMapper : Mapper<ChatEndpointRequest, ChatResponse, ChatResultDto>
{
  public override ChatResponse FromEntity(ChatResultDto e) =>
    new(e.ConversationId, e.Reply, e.Model);
}
