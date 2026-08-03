using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using System;

OllamaApiClient ollama = new OllamaApiClient(new OllamaApiClient.Configuration
{
    Uri = new Uri("http://localhost:11434"),
    Model = "qwen2.5:3b",
});

AIAgent agent = ollama.AsAIAgent(
    instructions: "You are a helpful assistant running locally via Ollama.");

Console.WriteLine(await agent.RunAsync("What is the largest city in France?"));