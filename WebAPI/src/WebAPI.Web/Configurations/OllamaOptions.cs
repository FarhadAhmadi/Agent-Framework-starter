namespace WebAPI.Web.Configurations;

public sealed class OllamaOptions
{
  public const string SectionName = "Ollama";
  public string Endpoint { get; set; } = "http://localhost:11434";
  public string DefaultModel { get; set; } = "qwen2.5:3b";
  public string DefaultSystemPrompt { get; set; } = "You are a helpful assistant.";
}
