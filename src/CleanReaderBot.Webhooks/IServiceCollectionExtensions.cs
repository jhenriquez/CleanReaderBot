using Microsoft.Extensions.DependencyInjection;
using CleanReaderBot.Webhooks.Services;
using Telegram.Bot;

namespace CleanReaderBot.Webhooks
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddCleanReaderBotWebhooksTelegramIntegration(this IServiceCollection services)
    {
      services.AddSingleton<ISpecificBotService<TelegramBotClient, TelegramSettings>, TelegramBotService>();
      return services;
    }
  }
}