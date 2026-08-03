using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OllamaSharp;
using WebAPI.Web.AiFeatures.Infrastructure;

namespace WebAPI.Web.Configurations;

public static class AiServiceConfigs
{
  public static IServiceCollection AddAiServiceConfigs(
    this IServiceCollection services, IConfiguration configuration)
  {
    services.Configure<OllamaOptions>(configuration.GetSection(OllamaOptions.SectionName));

    services.AddSingleton<IChatClient>(sp =>
    {
      var options = sp.GetRequiredService<IOptions<OllamaOptions>>().Value;
      return new OllamaApiClient(new Uri(options.Endpoint), options.DefaultModel);
    });

    // In-memory store: singleton, thread-safe. Replace with a persistent
    // aggregate + EfRepository for production (see note below).
    services.AddSingleton<IConversationStore, InMemoryConversationStore>();

    return services;
  }
}
