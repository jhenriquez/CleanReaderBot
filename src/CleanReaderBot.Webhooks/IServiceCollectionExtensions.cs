using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using CleanReaderBot.Webhooks.Services;
using CleanReaderBot.Webhooks.Models;

namespace CleanReaderBot.Webhooks
{
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection AddCleanReaderBotWebhooksTelegramIntegration(this IServiceCollection services)
    {
      services.AddSingleton<TelegramBotService>();
      services.AddSingleton<IBotService>(x => x.GetRequiredService<TelegramBotService>());
      services.AddSingleton<ISpecificBotService<TelegramBotClient, TelegramSettings>>(x => x.GetRequiredService<TelegramBotService>());
      return services;
    }
  }
}