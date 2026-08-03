namespace WebAPI.Web.AiFeatures;

// Internal (handler) contracts
public record ChatResultDto(Guid ConversationId, string Reply, string Model);

// API response contracts
public record ChatResponse(Guid ConversationId, string Reply, string Model);
public record MessageResponse(string Role, string Text);
public record ConversationSummaryResponse(Guid Id, DateTimeOffset CreatedAt, int MessageCount);
