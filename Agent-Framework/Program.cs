// Program.cs
using Agent_Framework;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OllamaSharp;

var services = new ServiceCollection();

// Register Ollama as the IChatClient implementation
services.AddScoped<IChatClient>(_ =>
    new OllamaApiClient(new Uri("http://localhost:11434"), "qwen2.5:3b"));

// Register the AI service abstraction
services.AddScoped<IAIService, OllamaAgentService>();

var provider = services.BuildServiceProvider();

var ai = provider.GetRequiredService<IAIService>();
var result = await ai.GetResponseAsync("What is the largest city in France?");
Console.WriteLine(result);
